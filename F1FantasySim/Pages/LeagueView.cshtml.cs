using F1FantasySim.Models;
using F1FantasySim.Models.NewAPI;
using F1FantasySim.Models.NewAPI.LeagueView;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;

namespace F1FantasySim.Pages
{
    public class LeagueViewModel : PageModel
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3";
        private const string DriverURL = "https://fantasy.formula1.com/feeds/drivers/";
        private const string LeagueURL = "https://fantasy.formula1.com/services/user/leaderboard/87eedb8a-d4a9-11ee-938e-5f8047180a36/pvtleagueuserrankget/1/353108/0/1/1/200/";

        private readonly ApiSettings _apiSettings;
        private readonly HttpClient _httpClient;
        private readonly Utils _utils;

        public LeagueApiModel League { get; private set; }
        public List<PlayerDetails> Players { get; private set; }
        public List<int> AvailableRaceIds { get; set; } = new List<int>(); // Assume this gets populated with actual race IDs
        public int SelectedRaceId { get; set; }

        public LeagueViewModel(IOptions<ApiSettings> options)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            _utils = new Utils();
            _apiSettings = options.Value;
        }

        public async Task OnGetAsync(int? raceId)
        {
            // Fetch the upcoming race ID. If raceId query parameter is provided, use it; otherwise, fetch the default upcoming race ID.
            var upcomingRaceId = await _utils.GetUpcomingRace(_httpClient);

            // Ensure the SelectedRaceId property is set to the used race ID
            SelectedRaceId = raceId ?? upcomingRaceId;

            // Populate AvailableRaceIds with IDs from 1 to upcomingRaceId
            AvailableRaceIds = Enumerable.Range(1, upcomingRaceId).ToList();

            string driversUrl = $"{DriverURL}{SelectedRaceId}_en.json";

            Console.WriteLine($"Player Cookies {_apiSettings.PlayerCookies}");
            Console.WriteLine($"League Cookies {_apiSettings.LeagueCookies}");

            var driversAndConstructorsRequest = CreateHttpRequestMessage(driversUrl, null); // No cookies needed for this request
            var driversAndConstructors = await GetApiDataUsingRequest<List<DriverApiModel>>(driversAndConstructorsRequest);

            var leagueRequest = CreateHttpRequestMessage(LeagueURL, _apiSettings.LeagueCookies);
            League = await GetApiDataUsingRequest<LeagueApiModel>(leagueRequest);


            Players = new List<PlayerDetails>();
            foreach (var member in League.memRank)
            {
                var memberUri = $"https://fantasy.formula1.com/services/user/opponentteam/opponentgamedayplayerteamget/1/{member.guid}/{member.teamNo}/{SelectedRaceId}/1";
                var playerRequest = CreateHttpRequestMessage(memberUri, _apiSettings.PlayerCookies);

                var playerDetails = await GetApiDataUsingRequest<PlayerDetails>(playerRequest);
                Players.Add(playerDetails);
            }

            UpdatePlayers(Players, driversAndConstructors);
        }

        private void UpdatePlayers(List<PlayerDetails> players, List<DriverApiModel> driversAndConstructors)
        {
            foreach (var playerDetail in Players) // Assuming Players is a list of PlayerDetails
            {
                foreach (var userTeam in playerDetail.userTeam)
                {
                    userTeam.teamname = WebUtility.UrlDecode(userTeam.teamname);
                    foreach (var player in userTeam.playerid)
                    {
                        // Find the corresponding DriverApiModel based on player ID
                        var driverInfo = driversAndConstructors.FirstOrDefault(d => d.PlayerId == player.id);
                        if (driverInfo != null)
                        {
                            // Assign the found DriverApiModel to the Player's DriverDetails property
                            player.DriverDetails = driverInfo;
                        }
                    }
                    userTeam.playerid = userTeam.playerid.OrderBy(a=> a.DriverDetails.IsConstructor()).ThenByDescending(a => a.DriverDetails.Value).ToList();
                }
            }
        }

        private HttpRequestMessage CreateHttpRequestMessage(string url, string cookies)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", UserAgent);
            if (!string.IsNullOrWhiteSpace(cookies))
            {
                AddCookiesToRequest(request, cookies);
            }
            return request;
        }

        private void AddCookiesToRequest(HttpRequestMessage request, string cookies)
        {
            if (!string.IsNullOrWhiteSpace(cookies))
            {
                request.Headers.Add("Cookie", cookies);
            }
        }

        private async Task<T> GetApiDataUsingRequest<T>(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(content);
                return jsonObject["Data"]["Value"].ToObject<T>();
            }
            return default;
        }
    }
}
