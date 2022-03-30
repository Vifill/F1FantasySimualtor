using F1FantasySim.Models;

namespace F1FantasySim
{
    public class SimulatorV2
    {
        public List<ApiModel> RaceResult;

        private readonly List<int> QualiPoints = new List<int>() { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        private readonly List<int> RacePoints = new List<int>() { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };

        private readonly int QualiBeatTeammatePoints = 2;
        private readonly int RaceBeatTeammatePoints = 3;

        public SimulatorV2(List<ApiModel> raceResult)
        {
            RaceResult = raceResult;
        }

        public Dictionary<ApiModel, int> CalculatePoints(List<ApiModel> team)
        {
            var drivers = team.Where(a => !a.is_constructor).ToList();

            Dictionary<ApiModel, int> appPlayerPoints = new Dictionary<ApiModel, int>();
            foreach(var driver in RaceResult.Where(a=> !a.is_constructor))
            {
                //give 1 point for finishing the race
                appPlayerPoints.Add(driver, 1);
            }

            CalculateQualifying(appPlayerPoints);
            CalculateRace(appPlayerPoints);
            CalculateTurboDriver(appPlayerPoints);

            var constructor = team.Single(a => a.is_constructor);
            appPlayerPoints.Add(constructor, appPlayerPoints.Where(a => a.Key.team_id == constructor.team_id).Sum(a => a.Value));

            var result = appPlayerPoints.Where(a => team.Select(b => b.id).Contains(a.Key.id)).ToDictionary(a => a.Key, b => b.Value);
            result.Comparer
            return 
        }

        private void CalculateTurboDriver(Dictionary<ApiModel, int> appPlayerPoints)
        {
            foreach (var driver in appPlayerPoints)
            {

            }

        }

        private void CalculateQualifying(Dictionary<ApiModel, int> drivers)
        {
            CalculatePositionPoints(drivers, QualiPoints);
            CalcualteBeatTeammatePoints(drivers, QualiBeatTeammatePoints);
        }

        private void CalculateRace(Dictionary<ApiModel, int> drivers)
        {
            CalculatePositionPoints(drivers, RacePoints);
            CalcualteBeatTeammatePoints(drivers, RaceBeatTeammatePoints);
        }

        private void CalculatePositionPoints(Dictionary<ApiModel, int> drivers, List<int> pointsForPosition)
        {
            foreach (var key in drivers.Keys)
            {
                int position = RaceResult.IndexOf(key);
                if (position < 10)
                {
                    drivers[key] += pointsForPosition[position];
                }
            }
        }

        private void CalcualteBeatTeammatePoints(Dictionary<ApiModel, int> drivers, int pointsForbeatingTeammate)
        {
            foreach (var key in drivers.Keys)
            {
                drivers[key] += DidBeatTeammate(key) ? pointsForbeatingTeammate : 0;
            }
        }

        private bool DidBeatTeammate(ApiModel driver)
        {
            var teammate = RaceResult.Single(a => a.id != driver.id && a.team_id == driver.team_id);
            return RaceResult.IndexOf(driver) < RaceResult.IndexOf(teammate);
        }
    }
}
