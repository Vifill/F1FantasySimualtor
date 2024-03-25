using F1FantasySim.Models.NewAPI;
using F1FantasySim.Models.NewAPI.LeagueView;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net;

namespace F1FantasySim.Pages
{
    public class LeagueViewModel : PageModel
    {
        private const string PlayerCookies = "talkative_ecs__eu__260c846d-7b81-400a-b5e7-e4c3ec819064__widget_is_controlled_fullscreen=0; talkative_ecs__eu__260c846d-7b81-400a-b5e7-e4c3ec819064__push_prompt_dismissed=0; _rdt_uuid=1696608940891.94b3ce77-0193-454b-8cc9-1e631f372803; _scid=6ff81d14-8433-4efb-bb15-3f5620cb00dd; _sfid_8374={%22anonymousId%22:%22cab151d057818b5c%22%2C%22consents%22:[]}; _evga_95b0={%22uuid%22:%22cab151d057818b5c%22%2C%22puid%22:%22Dx3JX7h3pFba-AZ-pOWwnvUWLi6swarFDr8WW6dxRApFL_5m7x2W16YQZQHngn2MM_fordS9J9JuqJHUGJQ5rgyMEq9418S6tyte34ML11N7TGa4rlWhlmxrlcyikqtd%22%2C%22affinityId%22:%220B8%22}; _cb=DctueLRpP9R_wivL; _scid_r=6ff81d14-8433-4efb-bb15-3f5620cb00dd; _chartbeat2=.1698931481663.1704362253588.0000000000000001.Bxrx6EC0F3SBn732VC-AJoRClMPce.2; _ga_VWRQD933RZ=GS1.1.1704362246.4.1.1704372391.0.0.0; consentUUID=13311aed-34d5-4638-9866-fc6bc0feef7c_24_29; _gcl_au=1.1.561931165.1708508244; _ga=GA1.1.841651707.1696607071; _ga_KT1KWN44WH=GS1.1.1709126800.9.0.1709126800.0.0.0; consentDate=2024-03-06T11:22:19.899Z; si-en-info-popup=1711119046; isFirstRendering=false; reese84=3:Z2AipieEMSkbAwr9p3Q0GA==:9pFmIoDeb+VG/VFTVgxxFKyn4XM8tsn5x0qrd/zaNPStz8tDK0MBgeNax8AR0AxGMyz572lfbwzykQCiwqXqMM49t1f8TsyfK6pfuRzeEmDTLkcxyvutBHA2myrHWvSNnnl8tJw9YQ1P0tWMwk1UBzgrEnVAs7jNKLDzKOznVtJ8kHb3BXFhdOgkAIEyX6N9Ri4AoVgSwu2YSaHK3F4B35U2GQWkHXgA6tzEJW2QXSznkSfQUOOet8ttwonqxNHxLS/bMUmXf70R29P4/BuTGE7AfjcX2dbtsK8eP1nrg8sxh9Zpoe9QfGt6R6z4wDUJ2GSWNAtlmJHPYHIPHro9Y2thLbWZ1qoJp1KDcxesivKZzSMrXhE4Ubhw3j6hu47RGllj6uyl4i0HuyK0TyIirCIJPhHmIF7jkAE1QhAD+DWi4ne84yC43E1xLEoEqZ3/UOR8EcDoiD3r4zeepHOdHA==:q4rDO3txmGdkv3eoy32FajEPdOxrZ8Wn0Vc11WXHkRo=; login-session=%7B%22data%22%3A%7B%22subscriptionToken%22%3A%22eyJraWQiOiIxIiwidHlwIjoiSldUIiwiYWxnIjoiUlMyNTYifQ.eyJFeHRlcm5hbEF1dGhvcml6YXRpb25zQ29udGV4dERhdGEiOiJJU0wiLCJTdWJzY3JpcHRpb25TdGF0dXMiOiJpbmFjdGl2ZSIsIlN1YnNjcmliZXJJZCI6IjE1Njc2MTgwNyIsIkZpcnN0TmFtZSI6IlbDrWZpbGwiLCJlbnRzIjpbeyJjb3VudHJ5IjoiSVNMIiwiZW50IjoiUkVHIn1dLCJMYXN0TmFtZSI6IlZhbGRpbWFyc3NvbiIsImV4cCI6MTcxMTcwOTcwNSwiU2Vzc2lvbklkIjoiZXlKaGJHY2lPaUpvZEhSd09pOHZkM2QzTG5jekxtOXlaeTh5TURBeEx6QTBMM2h0YkdSemFXY3RiVzl5WlNOb2JXRmpMWE5vWVRJMU5pSXNJblI1Y0NJNklrcFhWQ0lzSW1OMGVTSTZJa3BYVkNKOS5leUppZFNJNklqRXdNREV4SWl3aWMya2lPaUkyTUdFNVlXUTROQzFsT1ROa0xUUTRNR1l0T0RCa05pMWhaak0zTkRrMFpqSmxNaklpTENKb2RIUndPaTh2YzJOb1pXMWhjeTU0Yld4emIyRndMbTl5Wnk5M2N5OHlNREExTHpBMUwybGtaVzUwYVhSNUwyTnNZV2x0Y3k5dVlXMWxhV1JsYm5ScFptbGxjaUk2SWpFMU5qYzJNVGd3TnlJc0ltbGtJam9pTXpVeU9XUXhNbUV0TURNNE5DMDBaR05rTFRnMVpqY3RNR1JqWXpBNVpHWmtPREV3SWl3aWRDSTZJakVpTENKc0lqb2laVzR0UjBJaUxDSmtZeUk2SWpNMk5EUWlMQ0poWldRaU9pSXlNREkwTFRBMExUQTRWREV3T2pVMU9qQTFMalkxTlZvaUxDSmtkQ0k2SWpFaUxDSmxaQ0k2SWpJd01qUXRNRFF0TWpSVU1UQTZOVFU2TURVdU5qVTFXaUlzSW1ObFpDSTZJakl3TWpRdE1ETXRNalpVTVRBNk5UVTZNRFV1TmpVMVdpSXNJbWx3SWpvaU1qRTNMamN3TGpJeE1DNDJNQ0lzSW1NaU9pSlRRMGhKVUVoUFRDSXNJbk4wSWpvaVRrZ2lMQ0p3WXlJNklqRXhNVGdpTENKamJ5STZJazVNUkNJc0ltNWlaaUk2TVRjeE1UTTJOREV3TlN3aVpYaHdJam94TnpFek9UVTJNVEExTENKcGMzTWlPaUpoYzJObGJtUnZiaTUwZGlJc0ltRjFaQ0k2SW1GelkyVnVaRzl1TG5SMkluMC4tSmMwcUMzRVNGSGFJWFlueElFaFRRQzdqbUxlSnpVMGZnZV9LbThJRWMwIiwiaWF0IjoxNzExMzY0MTA1LCJTdWJzY3JpYmVkUHJvZHVjdCI6IiIsImp0aSI6IjgzNzg1OWQ5LTY4Y2QtNDY5MS1iYjJhLWE4NTJjNjQ3MGZiMCJ9.EFFgjIvJuvSXjjIfuccOm1Y8-koAF1daKc3otj1DrYjOCrtZ4Nu2BH_5cKrqLU7JZXC7ACFSaAymykrV2J1HwolhXhWtEWUEy-0QhYTBGaHx_sCP7dCv8th7GslQMnBy3c1QjAoBnfbxsstWHU5_EI2GtJAyz56t_H-IMONJMsj-MCjO0GK6KU5EzZ_1iUbBtoYNJAH6sWRKoE4iQPwjgRin6_3tSmB0xdZHSpEYBy6tYu1cc6yDQG9_lWJWWFB8sGytRKyJcIPBBEmlekZ_TTKitvSTaslc9HqcPe2fKvJTJLjlt8SI2tHnGxGnO8NimBRg19F_M4fYVxyMMNN8Xg%22%7D%7D; user-metadata=%7B%22subscriptionSource%22%3A%22%22%2C%22userRegistrationLevel%22%3A%22full%22%2C%22subscribedProduct%22%3A%22%22%2C%22subscriptionExpiry%22%3A%2299%2F99%2F9999%22%7D; F1_FANTASY_007=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyIwMDciOiIxNTY3NjE4MDciLCJuYmYiOjE3MTEzNjQxMDYsImV4cCI6MTcxMTcwOTcwNiwiaWF0IjoxNzExMzY0MTA2fQ.4IPZbIS4bzwgQLt4aplaA0zRbgA3KlKvPD4y-0CdhKo;";
        private const string LeagueCookies = "talkative_ecs__eu__260c846d-7b81-400a-b5e7-e4c3ec819064__widget_is_controlled_fullscreen=0; talkative_ecs__eu__260c846d-7b81-400a-b5e7-e4c3ec819064__push_prompt_dismissed=0; _rdt_uuid=1696608940891.94b3ce77-0193-454b-8cc9-1e631f372803; _scid=6ff81d14-8433-4efb-bb15-3f5620cb00dd; _sfid_8374={%22anonymousId%22:%22cab151d057818b5c%22%2C%22consents%22:[]}; _evga_95b0={%22uuid%22:%22cab151d057818b5c%22%2C%22puid%22:%22Dx3JX7h3pFba-AZ-pOWwnvUWLi6swarFDr8WW6dxRApFL_5m7x2W16YQZQHngn2MM_fordS9J9JuqJHUGJQ5rgyMEq9418S6tyte34ML11N7TGa4rlWhlmxrlcyikqtd%22%2C%22affinityId%22:%220B8%22}; _cb=DctueLRpP9R_wivL; _scid_r=6ff81d14-8433-4efb-bb15-3f5620cb00dd; _chartbeat2=.1698931481663.1704362253588.0000000000000001.Bxrx6EC0F3SBn732VC-AJoRClMPce.2; _ga_VWRQD933RZ=GS1.1.1704362246.4.1.1704372391.0.0.0; consentUUID=13311aed-34d5-4638-9866-fc6bc0feef7c_24_29; _gcl_au=1.1.561931165.1708508244; _ga=GA1.1.841651707.1696607071; _ga_KT1KWN44WH=GS1.1.1709126800.9.0.1709126800.0.0.0; consentDate=2024-03-06T11:22:19.899Z; si-en-info-popup=1711119046; isFirstRendering=false; reese84=3:Z2AipieEMSkbAwr9p3Q0GA==:9pFmIoDeb+VG/VFTVgxxFKyn4XM8tsn5x0qrd/zaNPStz8tDK0MBgeNax8AR0AxGMyz572lfbwzykQCiwqXqMM49t1f8TsyfK6pfuRzeEmDTLkcxyvutBHA2myrHWvSNnnl8tJw9YQ1P0tWMwk1UBzgrEnVAs7jNKLDzKOznVtJ8kHb3BXFhdOgkAIEyX6N9Ri4AoVgSwu2YSaHK3F4B35U2GQWkHXgA6tzEJW2QXSznkSfQUOOet8ttwonqxNHxLS/bMUmXf70R29P4/BuTGE7AfjcX2dbtsK8eP1nrg8sxh9Zpoe9QfGt6R6z4wDUJ2GSWNAtlmJHPYHIPHro9Y2thLbWZ1qoJp1KDcxesivKZzSMrXhE4Ubhw3j6hu47RGllj6uyl4i0HuyK0TyIirCIJPhHmIF7jkAE1QhAD+DWi4ne84yC43E1xLEoEqZ3/UOR8EcDoiD3r4zeepHOdHA==:q4rDO3txmGdkv3eoy32FajEPdOxrZ8Wn0Vc11WXHkRo=; login-session=%7B%22data%22%3A%7B%22subscriptionToken%22%3A%22eyJraWQiOiIxIiwidHlwIjoiSldUIiwiYWxnIjoiUlMyNTYifQ.eyJFeHRlcm5hbEF1dGhvcml6YXRpb25zQ29udGV4dERhdGEiOiJJU0wiLCJTdWJzY3JpcHRpb25TdGF0dXMiOiJpbmFjdGl2ZSIsIlN1YnNjcmliZXJJZCI6IjE1Njc2MTgwNyIsIkZpcnN0TmFtZSI6IlbDrWZpbGwiLCJlbnRzIjpbeyJjb3VudHJ5IjoiSVNMIiwiZW50IjoiUkVHIn1dLCJMYXN0TmFtZSI6IlZhbGRpbWFyc3NvbiIsImV4cCI6MTcxMTcwOTcwNSwiU2Vzc2lvbklkIjoiZXlKaGJHY2lPaUpvZEhSd09pOHZkM2QzTG5jekxtOXlaeTh5TURBeEx6QTBMM2h0YkdSemFXY3RiVzl5WlNOb2JXRmpMWE5vWVRJMU5pSXNJblI1Y0NJNklrcFhWQ0lzSW1OMGVTSTZJa3BYVkNKOS5leUppZFNJNklqRXdNREV4SWl3aWMya2lPaUkyTUdFNVlXUTROQzFsT1ROa0xUUTRNR1l0T0RCa05pMWhaak0zTkRrMFpqSmxNaklpTENKb2RIUndPaTh2YzJOb1pXMWhjeTU0Yld4emIyRndMbTl5Wnk5M2N5OHlNREExTHpBMUwybGtaVzUwYVhSNUwyTnNZV2x0Y3k5dVlXMWxhV1JsYm5ScFptbGxjaUk2SWpFMU5qYzJNVGd3TnlJc0ltbGtJam9pTXpVeU9XUXhNbUV0TURNNE5DMDBaR05rTFRnMVpqY3RNR1JqWXpBNVpHWmtPREV3SWl3aWRDSTZJakVpTENKc0lqb2laVzR0UjBJaUxDSmtZeUk2SWpNMk5EUWlMQ0poWldRaU9pSXlNREkwTFRBMExUQTRWREV3T2pVMU9qQTFMalkxTlZvaUxDSmtkQ0k2SWpFaUxDSmxaQ0k2SWpJd01qUXRNRFF0TWpSVU1UQTZOVFU2TURVdU5qVTFXaUlzSW1ObFpDSTZJakl3TWpRdE1ETXRNalpVTVRBNk5UVTZNRFV1TmpVMVdpSXNJbWx3SWpvaU1qRTNMamN3TGpJeE1DNDJNQ0lzSW1NaU9pSlRRMGhKVUVoUFRDSXNJbk4wSWpvaVRrZ2lMQ0p3WXlJNklqRXhNVGdpTENKamJ5STZJazVNUkNJc0ltNWlaaUk2TVRjeE1UTTJOREV3TlN3aVpYaHdJam94TnpFek9UVTJNVEExTENKcGMzTWlPaUpoYzJObGJtUnZiaTUwZGlJc0ltRjFaQ0k2SW1GelkyVnVaRzl1TG5SMkluMC4tSmMwcUMzRVNGSGFJWFlueElFaFRRQzdqbUxlSnpVMGZnZV9LbThJRWMwIiwiaWF0IjoxNzExMzY0MTA1LCJTdWJzY3JpYmVkUHJvZHVjdCI6IiIsImp0aSI6IjgzNzg1OWQ5LTY4Y2QtNDY5MS1iYjJhLWE4NTJjNjQ3MGZiMCJ9.EFFgjIvJuvSXjjIfuccOm1Y8-koAF1daKc3otj1DrYjOCrtZ4Nu2BH_5cKrqLU7JZXC7ACFSaAymykrV2J1HwolhXhWtEWUEy-0QhYTBGaHx_sCP7dCv8th7GslQMnBy3c1QjAoBnfbxsstWHU5_EI2GtJAyz56t_H-IMONJMsj-MCjO0GK6KU5EzZ_1iUbBtoYNJAH6sWRKoE4iQPwjgRin6_3tSmB0xdZHSpEYBy6tYu1cc6yDQG9_lWJWWFB8sGytRKyJcIPBBEmlekZ_TTKitvSTaslc9HqcPe2fKvJTJLjlt8SI2tHnGxGnO8NimBRg19F_M4fYVxyMMNN8Xg%22%7D%7D; user-metadata=%7B%22subscriptionSource%22%3A%22%22%2C%22userRegistrationLevel%22%3A%22full%22%2C%22subscribedProduct%22%3A%22%22%2C%22subscriptionExpiry%22%3A%2299%2F99%2F9999%22%7D; F1_FANTASY_007=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyIwMDciOiIxNTY3NjE4MDciLCJuYmYiOjE3MTEzNjQxMDYsImV4cCI6MTcxMTcwOTcwNiwiaWF0IjoxNzExMzY0MTA2fQ.4IPZbIS4bzwgQLt4aplaA0zRbgA3KlKvPD4y-0CdhKo;";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3";
        private const string DriverURL = "https://fantasy.formula1.com/feeds/drivers/";
        private const string LeagueURL = "https://fantasy.formula1.com/services/user/leaderboard/87eedb8a-d4a9-11ee-938e-5f8047180a36/pvtleagueuserrankget/1/353108/0/1/1/200/";

        private readonly HttpClient _httpClient;
        private readonly Utils _utils;

        public LeagueApiModel League { get; private set; }
        public List<PlayerDetails> Players { get; private set; }
        public List<int> AvailableRaceIds { get; set; } = new List<int>(); // Assume this gets populated with actual race IDs
        public int SelectedRaceId { get; set; }

        public LeagueViewModel()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            _utils = new Utils();
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

            var driversAndConstructorsRequest = CreateHttpRequestMessage(driversUrl, null); // No cookies needed for this request
            var driversAndConstructors = await GetApiDataUsingRequest<List<DriverApiModel>>(driversAndConstructorsRequest);

            var leagueRequest = CreateHttpRequestMessage(LeagueURL, LeagueCookies);
            League = await GetApiDataUsingRequest<LeagueApiModel>(leagueRequest);

            Players = new List<PlayerDetails>();
            foreach (var member in League.memRank)
            {
                var memberUri = $"https://fantasy.formula1.com/services/user/opponentteam/opponentgamedayplayerteamget/1/{member.guid}/{member.teamNo}/{SelectedRaceId}/1";
                var playerRequest = CreateHttpRequestMessage(memberUri, PlayerCookies);

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
