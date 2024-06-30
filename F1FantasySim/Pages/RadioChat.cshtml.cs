using System.IO;
using System.Text.Json;
using System.Web; // Add this for HtmlUtility
using F1FantasySim.Models;
using F1FantasySim.Models.FormModel;
using F1FantasySim.Models.RadioChat;
using F1FantasySim.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OpenAI.Audio;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace F1FantasySim.Pages
{
    //[BindProperties]
    public class RadioChatModel : PageModel
    {
        private readonly ILogger<RadioChatModel> _logger;
        private readonly IConfiguration _configuration;
        [BindProperty]
        public RadioChatViewModel FormModel { get; set; }
        public Dictionary<string, List<SessionInfo>> CircuitSessions { get; set; }

        [BindNever]
        public Dictionary<string, List<DriverRecording>> DriverRecordings { get; set; }

        // Cache for transcriptions
        private readonly Dictionary<string, string> _transcriptionCache = new Dictionary<string, string>();
        private readonly string _cacheFilePath = "transcriptions.json";

        public RadioChatModel(ILogger<RadioChatModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            CircuitSessions = new Dictionary<string, List<SessionInfo>>();
            DriverRecordings = new Dictionary<string, List<DriverRecording>>();
            FormModel = new RadioChatViewModel();

            // Load cache on startup
            LoadTranscriptionCache();
        }

        public async Task OnGet()
        {
            await FetchSessionDataAsync();
        }

        public async Task<IActionResult> OnPostAsync(RadioChatViewModel formModel)
        {
            FormModel = formModel;
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
            await FetchSessionDataAsync();

            try
            {
                string driversApiUrl = $"https://api.openf1.org/v1/drivers?session_key={FormModel.SelectedSession}&team_name={Uri.EscapeDataString(FormModel.SelectedTeam)}";
                var driversResponse = await httpClient.GetStringAsync(driversApiUrl);
                var drivers = JsonSerializer.Deserialize<List<Driver>>(driversResponse);

                foreach (var driver in drivers)
                {
                    string teamRadioApiUrl = $"https://api.openf1.org/v1/team_radio?session_key={FormModel.SelectedSession}&driver_number={driver.DriverNumber}";
                    var teamRadioResponse = await httpClient.GetStringAsync(teamRadioApiUrl);
                    var teamRadios = JsonSerializer.Deserialize<List<TeamRadio>>(teamRadioResponse);

                    foreach (var teamRadio in teamRadios)
                    {
                        if (!DriverRecordings.ContainsKey(driver.FullName))
                        {
                            DriverRecordings[driver.FullName] = new List<DriverRecording>();
                        }

                        // Get the transcription
                        string transcription = await GetTranscriptionAsync(teamRadio.RecordingUrl);
                        // Decode the transcription to fix encoding issues
                        transcription = HttpUtility.HtmlDecode(transcription);
                        var driverRecording = new DriverRecording
                        {
                            RecordingUrl = teamRadio.RecordingUrl,
                            Transcription = transcription,
                            Date = teamRadio.Date
                        };

                        DriverRecordings[driver.FullName].Add(driverRecording);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching driver recordings.");
            }
        }

        private async Task<string> GetTranscriptionAsync(string url)
        {
            if (_transcriptionCache.ContainsKey(url))
            {
                return _transcriptionCache[url];
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (Stream audioStream = await client.GetStreamAsync(url))
                    {
                        AudioClient audioClient = new AudioClient("whisper-1", _configuration["OPENAI_API_KEY"]);
                        var text = await audioClient.TranscribeAudioAsync(audioStream, Path.GetFileName(url));
                        return text.Value.Text;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while transcribing audio.");
                return null;
            }
        }

        private void LoadTranscriptionCache()
        {
            if (System.IO.File.Exists(_cacheFilePath))
            {
                var json = System.IO.File.ReadAllText(_cacheFilePath);
                var cache = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (cache != null)
                {
                    foreach (var entry in cache)
                    {
                        _transcriptionCache[entry.Key] = entry.Value;
                    }
                }
            }
        }

        private void SaveTranscriptionCache()
        {
            var json = JsonSerializer.Serialize(_transcriptionCache);
            System.IO.File.WriteAllText(_cacheFilePath, json);
        }

        public record SessionInfo(string SessionName, int SessionKey);

        public record DriverRecording
        {
            public string RecordingUrl { get; set; }
            public string Transcription { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
