using F1FantasySim.Models.NewAPI;
using F1FantasySim.Models.NewAPI.LeagueView;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;

namespace F1FantasySim
{
    public class Utils
    {
        private readonly string ScheduleURL = "https://fantasy.formula1.com/feeds/schedule/raceday_en.json";

        public async Task<int> GetUpcomingRace(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(ScheduleURL); // Assume ScheduleURL is a valid URL defined elsewhere
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var parsed = JObject.Parse(result)["Data"]["Value"];
                var races = parsed.ToObject<List<RaceInfo>>();
                return races.FirstOrDefault(a => a.GDStatus is 1 or 2 or 3).GamedayId; // Crash if this fails, I want to know
            }
            else
            {
                Console.WriteLine($"Failed to retrieve race data: {response.StatusCode}");
                throw new Exception("Failed to retrieve race data");
            }
        }


    }
}
