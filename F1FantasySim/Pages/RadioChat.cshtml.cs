using System.Text.Json;
using F1FantasySim.Models;
using F1FantasySim.Models.RadioChat;
using F1FantasySim.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenAI.Audio;

namespace F1FantasySim.Pages
{
    [BindProperties]
    public class RadioChatModel : PageModel
    {
        private readonly ILogger<RadioChatModel> _logger;
        private readonly IConfiguration _configuration;

        public Dictionary<string, List<SessionInfo>> CircuitSessions { get; set; }
        public string SelectedCircuit { get; set; }
        public int SelectedSession { get; set; }
        public string SelectedTeam { get; set; }
        public Dictionary<string, string> DriverRecordings { get; set; }

        public RadioChatModel(ILogger<RadioChatModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            CircuitSessions = new Dictionary<string, List<SessionInfo>>();
            DriverRecordings = new Dictionary<string, string>();
        }

        public async Task OnGet()
        {
            await FetchSessionDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await FetchDriverRecordingsAsync();
            return Page();
        }

        private async Task FetchSessionDataAsync()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            try
            {
                string apiUrl = "https://api.openf1.org/v1/sessions?session_type=Practice&year=2024";
                var response = await httpClient.GetStringAsync(apiUrl);
                var sessions = JsonSerializer.Deserialize<List<Session>>(response);

                foreach (var session in sessions)
                {
                    string key = $"{session.CountryName}, {session.CircuitShortName}";
                    if (!CircuitSessions.ContainsKey(key))
                    {
                        CircuitSessions[key] = new List<SessionInfo>();
                    }
                    CircuitSessions[key].Add(new SessionInfo(session.SessionName, session.SessionKey));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching session data.");
            }
        }

        private async Task FetchDriverRecordingsAsync()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            try
            {
                string driversApiUrl = $"https://api.openf1.org/v1/drivers?session_key={SelectedSession}&team_name={Uri.EscapeDataString(SelectedTeam)}";
                var driversResponse = await httpClient.GetStringAsync(driversApiUrl);
                var drivers = JsonSerializer.Deserialize<List<Driver>>(driversResponse);

                foreach (var driver in drivers)
                {
                    string teamRadioApiUrl = $"https://api.openf1.org/v1/team_radio?session_key={SelectedSession}&driver_number={driver.DriverNumber}";
                    var teamRadioResponse = await httpClient.GetStringAsync(teamRadioApiUrl);
                    var teamRadios = JsonSerializer.Deserialize<List<TeamRadio>>(teamRadioResponse);

                    foreach (var teamRadio in teamRadios)
                    {
                        DriverRecordings[driver.FullName] = teamRadio.RecordingUrl;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching driver recordings.");
            }
        }

        public record SessionInfo(string SessionName, int SessionKey);
        


        private async Task GetRadioChatAsync()
        {
            try
            {
                string url = "https://livetiming.formula1.com/static/2023/2023-09-17_Singapore_Grand_Prix/2023-09-15_Practice_1/TeamRadio/SERPER01_11_20230915_104008.mp3";

                using (HttpClient client = new HttpClient())
                {
                    using (Stream audioStream = await client.GetStreamAsync(url))
                    {
                        AudioClient audioClient = new AudioClient("whisper-1", _configuration["OPENAI_API_KEY"]);
                        var text = await audioClient.TranscribeAudioAsync(audioStream, "SERPER01_11_20230915_104008.mp3");
                        Console.WriteLine(text);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}