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

		private readonly string DriverURL = "https://fantasy.formula1.com/feeds/drivers/";

        //public IEnumerable<PlayerApiModel>
        public List<DriverApiModel> Drivers { get; set; }
        public List<DriverApiModel> Constructors { get; set; }
        public List<CalculationTeamModel> BestTeams { get; set; }
        public List<CompetitorViewModel> DriverPoints{ get; set; }
        public List<CompetitorViewModel> ConstructorPoints { get; set; }

        public double MaxBudget { get; set; } = 103;

        private static Combinations<DriverApiModel> CombinationCache;

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
            Utils utils = new Utils();
			var upcomingRaceId = await utils.GetUpcomingRace(httpClient);

			// Construct the URL dynamically using the upcomingRaceId.
			string dynamicUrl = $"{DriverURL}{upcomingRaceId}_en.json";

			// Make the HTTP GET request to the constructed URL.
			var response = await httpClient.GetAsync(dynamicUrl);
			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();

                var playersAndConstructors = JObject.Parse(content)["Data"]["Value"].ToObject<List<DriverApiModel>>();
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

		public async Task<IActionResult> OnPost(int[] ids, int[] qualifyingIds)
        {
            await GetApiDataAsync();

            var driverResult = ids.Select(a => Drivers.Single(b => int.Parse(b.PlayerId) == a)).ToList();
            var qualiResult = qualifyingIds.Select(a => Drivers.Single(b => int.Parse(b.PlayerId) == a)).ToList();

            Drivers = driverResult;
            SimulatorV2 simulator = new SimulatorV2(driverResult, qualiResult, Constructors);

            // Get all driver combinations
            if (CombinationCache == null || CombinationCache.Count == 0)
            {
                CombinationCache = new Combinations<DriverApiModel>(Drivers, 5);
            }

            // Create new teams from the driver combination for each pair of constructors
            List<List<DriverApiModel>> teamList = new List<List<DriverApiModel>>();
            var constructorPairs = new Combinations<DriverApiModel>(Constructors, 2);

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

            BestTeams = simulatedTeams.OrderByDescending(a => a.Sum(a => a.Points.TotalPoints)).Where(a=> !a.Any(a=> a.ApiModel.PlayerId == "121" && a.IsTurboed)).Take(10).Select(team =>
                new CalculationTeamModel(
                    team, // The entire team as CompetitorViewModels
                    team.Where(b => b.ApiModel.IsConstructor()).ToList(), // The constructors in the team
                    team.Sum(a => a.Points.TotalPoints), // Total points
                    team.Sum(b => b.ApiModel.Value) // Total value
                )).ToList();

            var allPlayers = simulator.GetAllPlayerPoints();

            DriverPoints = allPlayers.Where(a => !a.ApiModel.IsConstructor()).ToList();
            ConstructorPoints = allPlayers.Where(a => a.ApiModel.IsConstructor()).ToList();

            return Page();
        }
    }



}