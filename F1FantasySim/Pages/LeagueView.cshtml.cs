using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using F1FantasySim.Models.NewAPI;
using F1FantasySim.Models.NewAPI.LeagueView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace F1FantasySim.Pages
{
    public class LeagueViewModel : PageModel
    {
        public LeagueApiModel League { get; private set; }
        public List<PlayerDetails> Players { get; private set; }

        private readonly string DriverURL = "https://fantasy.formula1.com/feeds/drivers/";
        private readonly string LeagueURL = "https://fantasy.formula1.com/services/user/leaderboard/87eedb8a-d4a9-11ee-938e-5f8047180a36/pvtleagueuserrankget/1/353108/0/1/1/200/";


        public async Task OnGet()
        {
            var utils = new Utils();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
            
            var upcomingRaceId = await utils.GetUpcomingRace(httpClient);
            string dynamicUrl = $"{DriverURL}{upcomingRaceId}_en.json";

            var driversAndConstructors = new List<DriverApiModel>();
            var response = await httpClient.GetAsync(dynamicUrl);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                driversAndConstructors = JObject.Parse(content)["Data"]["Value"].ToObject<List<DriverApiModel>>();
            }

            var handler = GetHttpClientWithCookies(LeagueURL, "talkative_ecs__eu__260c846d-7b81-400a-b5e7-e4c3ec819064__widget_is_controlled_fullscreen=0; talkative_ecs__eu__260c846d-7b81-400a-b5e7-e4c3ec819064__push_prompt_dismissed=0; _rdt_uuid=1696608940891.94b3ce77-0193-454b-8cc9-1e631f372803; _scid=6ff81d14-8433-4efb-bb15-3f5620cb00dd; _sfid_8374={%22anonymousId%22:%22cab151d057818b5c%22%2C%22consents%22:[]}; _evga_95b0={%22uuid%22:%22cab151d057818b5c%22%2C%22puid%22:%22Dx3JX7h3pFba-AZ-pOWwnvUWLi6swarFDr8WW6dxRApFL_5m7x2W16YQZQHngn2MM_fordS9J9JuqJHUGJQ5rgyMEq9418S6tyte34ML11N7TGa4rlWhlmxrlcyikqtd%22%2C%22affinityId%22:%220B8%22}; _cb=DctueLRpP9R_wivL; _scid_r=6ff81d14-8433-4efb-bb15-3f5620cb00dd; _chartbeat2=.1698931481663.1704362253588.0000000000000001.Bxrx6EC0F3SBn732VC-AJoRClMPce.2; _ga_VWRQD933RZ=GS1.1.1704362246.4.1.1704372391.0.0.0; consentUUID=13311aed-34d5-4638-9866-fc6bc0feef7c_24_29; _gcl_au=1.1.561931165.1708508244; _ga=GA1.1.841651707.1696607071; _ga_KT1KWN44WH=GS1.1.1709126800.9.0.1709126800.0.0.0; consentDate=2024-03-06T11:22:19.899Z; reese84=3:kW8g0mfVbwMwAbyzV9towQ==:NY7pP0nsoS2qoHgEmWXAwJsjXK/q9t3LJ7sKWOaoam9/ZBSTJbrH2Pv9s3yJzpwVqZjyo3X5tW8tsSxDKUGDI9noJDtfT8btd04xh9Ps2RgCyeJHxN4r/TjjZlaZZ6GJVNgFMrXdzgEDFZu8WFS/krLYEr4iND4L7pbbuKWUKAhQcML+goFqsqpg59KwvlSkAqSSZJyO3IfG07OgMzeMBlBrYuxIVnhekBAKA1dziYipXkD0kteZWMJgjxJx1xujKFjc9HZka2uoetvOZFonWXwPyRO7EeRgzZCUyssmGBOLDvFutxQbnwKYldrJVfRsE83buy/JtVVi8cGMjXKxgCZ0T2/Ga+1g2XwruU6naVvoTaHGcu/gOs3Vm9TXyJ5tjfUB9jRH+FoXWa87B06IpaFOq6QPZnhkqipkclyJqZuU7ETopCUcPWgPCwJh9X+jVQlNjujER+thqU2ONOSGng==:Bfsq12okbC6vivzPIsCKDq9GNoYsv8A1xatDODz4fvs=; login-session=%7B%22data%22%3A%7B%22subscriptionToken%22%3A%22eyJraWQiOiIxIiwidHlwIjoiSldUIiwiYWxnIjoiUlMyNTYifQ.eyJFeHRlcm5hbEF1dGhvcml6YXRpb25zQ29udGV4dERhdGEiOiJJU0wiLCJTdWJzY3JpcHRpb25TdGF0dXMiOiJhY3RpdmUiLCJTdWJzY3JpYmVySWQiOiIxNTY3NjE4MDciLCJGaXJzdE5hbWUiOiJWw61maWxsIiwiZW50cyI6W3siY291bnRyeSI6IklTTCIsImVudCI6IlJFRyJ9LHsiY291bnRyeSI6IklTTCIsImVudCI6IlBSTyJ9XSwiTGFzdE5hbWUiOiJWYWxkaW1hcnNzb24iLCJleHAiOjE3MTEyODYyMjMsIlNlc3Npb25JZCI6ImV5SmhiR2NpT2lKb2RIUndPaTh2ZDNkM0xuY3pMbTl5Wnk4eU1EQXhMekEwTDNodGJHUnphV2N0Ylc5eVpTTm9iV0ZqTFhOb1lUSTFOaUlzSW5SNWNDSTZJa3BYVkNJc0ltTjBlU0k2SWtwWFZDSjkuZXlKaWRTSTZJakV3TURFeElpd2ljMmtpT2lJMk1HRTVZV1E0TkMxbE9UTmtMVFE0TUdZdE9EQmtOaTFoWmpNM05EazBaakpsTWpJaUxDSm9kSFJ3T2k4dmMyTm9aVzFoY3k1NGJXeHpiMkZ3TG05eVp5OTNjeTh5TURBMUx6QTFMMmxrWlc1MGFYUjVMMk5zWVdsdGN5OXVZVzFsYVdSbGJuUnBabWxsY2lJNklqRTFOamMyTVRnd055SXNJbWxrSWpvaU5qYzFaRFF5TWpjdFpHRTNPUzAwWXpObUxUZ3pZV0V0TlRjd1pHUTFORGcyWVRReUlpd2lkQ0k2SWpFaUxDSnNJam9pWlc0dFIwSWlMQ0prWXlJNklqTTJORFFpTENKaFpXUWlPaUl5TURJMExUQTBMVEF6VkRFek9qRTNPakF6TGpBM09Gb2lMQ0prZENJNklqRWlMQ0psWkNJNklqSXdNalF0TURRdE1UbFVNVE02TVRjNk1ETXVNRGM0V2lJc0ltTmxaQ0k2SWpJd01qUXRNRE10TWpGVU1UTTZNVGM2TURNdU1EYzRXaUlzSW1sd0lqb2lNakUzTGpjd0xqSXhNQzQxTVNJc0ltTWlPaUpUUTBoSlVFaFBUQ0lzSW5OMElqb2lUa2dpTENKd1l5STZJakV4TVRnaUxDSmpieUk2SWs1TVJDSXNJbTVpWmlJNk1UY3hNRGswTURZeU15d2laWGh3SWpveE56RXpOVE15TmpJekxDSnBjM01pT2lKaGMyTmxibVJ2Ymk1MGRpSXNJbUYxWkNJNkltRnpZMlZ1Wkc5dUxuUjJJbjAucF9oZ0NFM1gwWmVWM3hSYzhEZUtaZThVZ0syT3BrUUdsNm5BR29pdXNNRSIsImlhdCI6MTcxMDk0MDYyMywiU3Vic2NyaWJlZFByb2R1Y3QiOiJGMSBUViBQcm8gQW5udWFsIiwianRpIjoiYTEyNDZhNGItMWQ1Mi00MGVlLThkNjUtYmE4NzJmODE0Mjg0In0.oo4GxpC3LEJJAQ8ktbZh_Cy8TSlSMsSaOekoYhmlNA4Qw-CyCCthL0DAqJp_xlAh8lyNzBQQ_keNTSKN6wx_1vOuUmDNG5MLFujpn0hufxHTe3qE7qDdkLYI1C66kCR-xIPehaqPHULf4JUv0zq-t-jLNS4BNwNLhPfHTVFZk8s-hhVKyx3RTEkcFv5__NHfWuEo2m7z46GnOX8w52JJCkbBblQYLC_BaGQLDGCzbVoHLPdsdBrkRYJlcluiBXnXtdhFMTan1y4rON4170iBgAkvB0JOFShT88vtXjVkeRExSXqZRRbmVbdF8iGIb834rUd5hZekJI8MsiFRYJXC_g%22%7D%7D; user-metadata=%7B%22subscriptionSource%22%3A%22%22%2C%22userRegistrationLevel%22%3A%22full%22%2C%22subscribedProduct%22%3A%22F1%20TV%20Pro%20Annual%22%2C%22subscriptionExpiry%22%3A%2299%2F99%2F9999%22%7D; F1_FANTASY_007=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyIwMDciOiIxNTY3NjE4MDciLCJuYmYiOjE3MTA5NDA2MjQsImV4cCI6MTcxMTI4NjIyNCwiaWF0IjoxNzEwOTQwNjI0fQ.uo-C-7g3qpuYonNPn1MLdN3E__ldlbf5TpbqW9zN0Xo; si-mix-api-buster=20240318144843");

            LeagueApiModel apiModel;
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

                response = await client.GetAsync(LeagueURL);
                var content = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(content);

                apiModel = jsonObject["Data"]["Value"].ToObject<LeagueApiModel>();

            }

            List<PlayerDetails> players = new List<PlayerDetails>();

            foreach (var member in apiModel.memRank)
            {
                var memberUri = $"https://fantasy.formula1.com/services/user/opponentteam/opponentgamedayplayerteamget/1/{member.guid}/{member.teamNo}/{upcomingRaceId}/1";
                handler = GetHttpClientWithCookies(memberUri, "talkative_ecs__eu__260c846d-7b81-400a-b5e7-e4c3ec819064__widget_is_controlled_fullscreen=0; talkative_ecs__eu__260c846d-7b81-400a-b5e7-e4c3ec819064__push_prompt_dismissed=0; _rdt_uuid=1696608940891.94b3ce77-0193-454b-8cc9-1e631f372803; _scid=6ff81d14-8433-4efb-bb15-3f5620cb00dd; _sfid_8374={%22anonymousId%22:%22cab151d057818b5c%22%2C%22consents%22:[]}; _evga_95b0={%22uuid%22:%22cab151d057818b5c%22%2C%22puid%22:%22Dx3JX7h3pFba-AZ-pOWwnvUWLi6swarFDr8WW6dxRApFL_5m7x2W16YQZQHngn2MM_fordS9J9JuqJHUGJQ5rgyMEq9418S6tyte34ML11N7TGa4rlWhlmxrlcyikqtd%22%2C%22affinityId%22:%220B8%22}; _cb=DctueLRpP9R_wivL; _scid_r=6ff81d14-8433-4efb-bb15-3f5620cb00dd; _chartbeat2=.1698931481663.1704362253588.0000000000000001.Bxrx6EC0F3SBn732VC-AJoRClMPce.2; _ga_VWRQD933RZ=GS1.1.1704362246.4.1.1704372391.0.0.0; consentUUID=13311aed-34d5-4638-9866-fc6bc0feef7c_24_29; _gcl_au=1.1.561931165.1708508244; _ga=GA1.1.841651707.1696607071; _ga_KT1KWN44WH=GS1.1.1709126800.9.0.1709126800.0.0.0; consentDate=2024-03-06T11:22:19.899Z; login-session=%7B%22data%22%3A%7B%22subscriptionToken%22%3A%22eyJraWQiOiIxIiwidHlwIjoiSldUIiwiYWxnIjoiUlMyNTYifQ.eyJFeHRlcm5hbEF1dGhvcml6YXRpb25zQ29udGV4dERhdGEiOiJJU0wiLCJTdWJzY3JpcHRpb25TdGF0dXMiOiJhY3RpdmUiLCJTdWJzY3JpYmVySWQiOiIxNTY3NjE4MDciLCJGaXJzdE5hbWUiOiJWw61maWxsIiwiZW50cyI6W3siY291bnRyeSI6IklTTCIsImVudCI6IlJFRyJ9LHsiY291bnRyeSI6IklTTCIsImVudCI6IlBSTyJ9XSwiTGFzdE5hbWUiOiJWYWxkaW1hcnNzb24iLCJleHAiOjE3MTEzMDIwODIsIlNlc3Npb25JZCI6ImV5SmhiR2NpT2lKb2RIUndPaTh2ZDNkM0xuY3pMbTl5Wnk4eU1EQXhMekEwTDNodGJHUnphV2N0Ylc5eVpTTm9iV0ZqTFhOb1lUSTFOaUlzSW5SNWNDSTZJa3BYVkNJc0ltTjBlU0k2SWtwWFZDSjkuZXlKaWRTSTZJakV3TURFeElpd2ljMmtpT2lJMk1HRTVZV1E0TkMxbE9UTmtMVFE0TUdZdE9EQmtOaTFoWmpNM05EazBaakpsTWpJaUxDSm9kSFJ3T2k4dmMyTm9aVzFoY3k1NGJXeHpiMkZ3TG05eVp5OTNjeTh5TURBMUx6QTFMMmxrWlc1MGFYUjVMMk5zWVdsdGN5OXVZVzFsYVdSbGJuUnBabWxsY2lJNklqRTFOamMyTVRnd055SXNJbWxrSWpvaVpHTTROamhsTWpNdE1qTmhZUzAwTjJJeExXRXdOelF0WWpneVpqQXhOR0ZqTmpZMUlpd2lkQ0k2SWpFaUxDSnNJam9pWlc0dFIwSWlMQ0prWXlJNklqTTJORFFpTENKaFpXUWlPaUl5TURJMExUQTBMVEF6VkRFM09qUXhPakl5TGpZeU5Wb2lMQ0prZENJNklqRWlMQ0psWkNJNklqSXdNalF0TURRdE1UbFVNVGM2TkRFNk1qSXVOakkxV2lJc0ltTmxaQ0k2SWpJd01qUXRNRE10TWpGVU1UYzZOREU2TWpJdU5qSTFXaUlzSW1sd0lqb2lNakUzTGpjd0xqSXhNQzQxTVNJc0ltTWlPaUpUUTBoSlVFaFBUQ0lzSW5OMElqb2lUa2dpTENKd1l5STZJakV4TVRnaUxDSmpieUk2SWs1TVJDSXNJbTVpWmlJNk1UY3hNRGsxTmpRNE1pd2laWGh3SWpveE56RXpOVFE0TkRneUxDSnBjM01pT2lKaGMyTmxibVJ2Ymk1MGRpSXNJbUYxWkNJNkltRnpZMlZ1Wkc5dUxuUjJJbjAuZXhWX1RRVEpyVWN5dlFqM2pid0RzLV9JWHMtcFlJVXpTd1Bzc2dqNWY3cyIsImlhdCI6MTcxMDk1NjQ4MiwiU3Vic2NyaWJlZFByb2R1Y3QiOiJGMSBUViBQcm8gQW5udWFsIiwianRpIjoiZTJmOTI3NmQtMDdmMy00ZjFkLWI2ODItMDNlNWJhNWI4NzE3In0.BeQ-L5gwKJoFJLQhy05DSEfPwVllJUebPrWMAeoXQqZ-oCXLveoJKY-YtV1MQjdmI5q5F4DXjskK540z9GmBSt6UHpjACf3BvjYKvZSp3K-1diATJo1uHxR2akCJUZunpI5m-Ve7vCNCKlJQ8dg6XoI5UL2Nj47oaE2oGwpUKeeL5poU5P5BX-uLukIbXKLFBMgwuecQV_gqOYmjc-02vojxozknSa26IkyuFZZ5SKvhz3MrOkga4cTPMI-iQRqGwX3MZOxHt13W68FGkMaz8vXBqfj0LQ__-1pdlsQsbcWniDN--Z--4VppPsML94COGJu2wsW6RLxJAv0gzOp27g%22%7D%7D; user-metadata=%7B%22subscriptionSource%22%3A%22%22%2C%22userRegistrationLevel%22%3A%22full%22%2C%22subscribedProduct%22%3A%22F1%20TV%20Pro%20Annual%22%2C%22subscriptionExpiry%22%3A%2299%2F99%2F9999%22%7D; F1_FANTASY_007=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyIwMDciOiIxNTY3NjE4MDciLCJuYmYiOjE3MTA5NTY0ODMsImV4cCI6MTcxMTMwMjA4MywiaWF0IjoxNzEwOTU2NDgzfQ.g74BRBRd2qzl0vpvvnOFjXXUhI331ud5Ak_S7_VZuKA; reese84=3:xDbo83TfNsZbLdBpB2XYnw==:ARW+qhnPXwABmjhv06K8k9RC4FURqotS//iCKzlbis2ICRXvnxLEcjNoI0JXdiYSPdpFLWTqgFo3g8PW+5OVbaosOnIlR8tSn+PlXU0fOTVfVuPMInwhUrCpeeGwk1c12DGWXX9etwFZpoJrnPNpxBQQMyGx5zBWe/2TxU2WSNaFrt2h75j0E0bkQ5D1m43jXLbBfsLTCaXtDVL8MycFvHddilHstjXCAYY6P8UTozz9kiJx6ZLwlL4KOPA37Yj9BaZGlp3fdf1y7eoyTYj0iO+T3UvfLv2AA3EAz7ebHCx32Aext75wUiIRsavg5CDiSw4DckNgKAlyv8cTlRtK4RVKH9dT/61M2pPa6pZPXWzIRqxPSM9F3hvK4Icno4S3CF/sR50FeuErqt8mgTTU8Wyv1MaZGKAO7rOA7UHlJs1qTSMV6LcJbfjuRdKeQrww1B/urXiZHGg0+xBsg/BNMg==:T9dDKZBKMb8fMDwuc8CXjtkELqxDXlk3KQdgB6jDi/8=; si-mix-api-buster=20240318144843");
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

                    response = await client.GetAsync(memberUri);
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(content);

                    var playerDetails = jsonObject["Data"]["Value"].ToObject<PlayerDetails>();
                    players.Add(playerDetails);
                }
            }

            var leagueViewModel = new LeagueViewModel
            {
                League = apiModel,
                Players = players
            };
        }

        private HttpClientHandler GetHttpClientWithCookies(string url, string cookies)
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            var uri = new Uri(url);
            foreach (var cookieStr in cookies.Split(';'))
            {
                var cookieParts = cookieStr.Split('=', 2); // Split each cookie into name and value
                if (cookieParts.Length >= 2) // Ensure the cookie has both a name and a value
                {
                    var cookieName = cookieParts[0].Trim();
                    var cookieValue = cookieParts[1].Trim();
                    handler.CookieContainer.Add(uri, new Cookie(cookieName, cookieValue));
                }
            }
            return handler;
        }
    }
}
