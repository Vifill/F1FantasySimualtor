using Combinatorics.Collections;
using F1FantasySim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace F1FantasySim.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly string URL = "https://fantasy-api.formula1.com/f1/2022/players";

        public IEnumerable<ApiModel> Drivers { get; set; }
        public IEnumerable<ApiModel> Constructors { get; set; }
        public List<CalculationTeamModel> BestTeams { get; set; }

        public double MaxBudget { get; set; } = 103;

        private static Combinations<ApiModel> CombinationCache;

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
            var result = await httpClient.GetAsync(URL).Result.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<Players>(result);

            Drivers = players.players.Where(a => !a.is_constructor).OrderByDescending(a=> a.season_score);
            Constructors = players.players.Where(a => a.is_constructor).OrderByDescending(a => a.season_score);
        }

        public async Task<IActionResult> OnPost(int[] ids)
        {
            await GetApiDataAsync();

            var raceResult = ids.Select(a => Drivers.Single(b => b.id == a)).ToList();
            Drivers = raceResult;
            SimulatorV2 simulator = new SimulatorV2(raceResult);

            //Get all driver combinations
            if(CombinationCache == null || CombinationCache.Count == 0)
            {
                CombinationCache = new Combinations<ApiModel>(Drivers, 5);
            }

            //Create new teams from the driver combination for each constructor
            List<List<ApiModel>> teamList = new List<List<ApiModel>>();
            foreach (var driverComb in CombinationCache)
            {
                foreach (var constructor in Constructors)
                {
                    var tmp = driverComb.ToList();
                    tmp.Add(constructor);
                    teamList.Add(tmp);
                }
            }

            var filteredlist = teamList.Where(a => a.Sum(b => b.price) < MaxBudget);

            List<Dictionary<ApiModel, int>> simulatedTeams = new List<Dictionary<ApiModel, int>>();
            foreach(var team in filteredlist)
            {
                var simulatedResult = simulator.CalculatePoints(team);
                simulatedTeams.Add(simulatedResult);
            }

            BestTeams = simulatedTeams.OrderByDescending(team => team.Sum(a => a.Value)).Take(10).Select(a=> 
            new CalculationTeamModel(
                a.Keys.Where(b => !b.is_constructor).ToList(), 
                a.Keys.Single(b=> b.is_constructor), 
                a.Values.Sum(), 
                a.Keys.Sum(b => b.price))).ToList();

            return Page();
        }
    }



}