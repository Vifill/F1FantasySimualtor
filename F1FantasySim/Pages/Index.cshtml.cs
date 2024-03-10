using Combinatorics.Collections;
using F1FantasySim.Models;
using F1FantasySim.Models.NewAPI;
using F1FantasySim.Models.OldAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace F1FantasySim.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly string ScheduleURL = "https://fantasy.formula1.com/feeds/schedule/raceday_en.json";

		private readonly string DriverURL = "https://fantasy.formula1.com/feeds/drivers/";

        //public IEnumerable<PlayerApiModel>
        public List<PlayerApiModel> Drivers { get; set; }
        public List<PlayerApiModel> Constructors { get; set; }
        public List<CalculationTeamModel> BestTeams { get; set; }

        public double MaxBudget { get; set; } = 103;

        private static Combinations<PlayerApiModel> CombinationCache;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            await GetApiDataAsync();
        }

        private async Task GetApiDataAsync()
		{
			HttpClient httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

			// Get the ID of the upcoming race.
			var upcomingRaceId = await GetUpcomingRace(httpClient);

			// Construct the URL dynamically using the upcomingRaceId.
			string dynamicUrl = $"{DriverURL}{upcomingRaceId}_en.json";

			// Make the HTTP GET request to the constructed URL.
			var response = await httpClient.GetAsync(dynamicUrl);
			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();

                var playersAndConstructors = JObject.Parse(content)["Data"]["Value"].ToObject<List<PlayerApiModel>>();
                Drivers = playersAndConstructors.Where(a => a.PositionName.Equals("DRIVER", StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(a=> double.Parse(a.OverallPpints)).Where(a=> a.PlayerId != "11031").ToList();
                Drivers.ForEach(a => a.PointsPerMillion(upcomingRaceId - 1));
                Constructors = playersAndConstructors.Where(a => a.PositionName.Equals("CONSTRUCTOR", StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
			else
			{
				Console.WriteLine($"Failed to retrieve data: {response.StatusCode}");
                throw new Exception("Failed to retrieve data");
			}
		}

		private async Task<int> GetUpcomingRace(HttpClient httpClient)
		{
			var response = await httpClient.GetAsync(ScheduleURL); // Assume ScheduleURL is a valid URL defined elsewhere
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				var parsed = JObject.Parse(result)["Data"]["Value"];
				var races = parsed.ToObject<List<RaceInfo>>();
				return races.FirstOrDefault(a => a.GDStatus == 1).GamedayId; // Crash if this fails, I want to know
			}
			else
			{
				Console.WriteLine($"Failed to retrieve race data: {response.StatusCode}");
                throw new Exception("Failed to retrieve race data");
			}
		}

		public async Task<IActionResult> OnPost(int[] ids, int[] qualifyingIds)
        {
            await GetApiDataAsync();

            var driverResult = ids.Select(a => Drivers.Single(b => int.Parse(b.PlayerId) == a)).ToList();
            var qualiResult = qualifyingIds.Select(a => Drivers.Single(b => int.Parse(b.PlayerId) == a)).ToList();

            Drivers = driverResult;
            SimulatorV2 simulator = new SimulatorV2(driverResult, qualiResult);

            // Get all driver combinations
            if (CombinationCache == null || CombinationCache.Count == 0)
            {
                CombinationCache = new Combinations<PlayerApiModel>(Drivers, 5);
            }

            // Create new teams from the driver combination for each pair of constructors
            List<List<PlayerApiModel>> teamList = new List<List<PlayerApiModel>>();
            var constructorPairs = new Combinations<PlayerApiModel>(Constructors, 2);

            foreach (var driverComb in CombinationCache)
            {
                foreach (var constructorPair in constructorPairs)
                {
                    var team = driverComb.Concat(constructorPair).ToList();
                    teamList.Add(team);
                }
            }

            var filteredList = teamList.Where(a => a.Sum(b => b.Value) < MaxBudget);

            List<List<CompetitorViewModel>> simulatedTeams = new List<List<CompetitorViewModel>>();
            foreach (var team in filteredList)
            {
                var simulatedResult = simulator.CalculatePoints(team);
                simulatedTeams.Add(simulatedResult);
            }

            BestTeams = simulatedTeams.OrderByDescending(a => a.Sum(a => a.Points.TotalPoints)).Take(10).Select(team =>
                new CalculationTeamModel(
                    team, // The entire team as CompetitorViewModels
                    team.Where(b => b.ApiModel.IsConstructor()).ToList(), // The constructors in the team
                    team.Sum(a => a.Points.TotalPoints), // Total points
                    team.Sum(b => b.ApiModel.Value) // Total value
                )).ToList();

            return Page();
        }
    }



}