using F1FantasySim.Models.OldAPI;

namespace F1FantasySim
{
    public class Simulator
    {
        public List<ApiModel> RaceResult;

        private readonly List<int> QualiPoints = new List<int>() { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        private readonly List<int> RacePoints = new List<int>() { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };

        private readonly int QualiBeatTeammatePoints = 2;
        private readonly int RaceBeatTeammatePoints = 2;

        public Simulator(List<ApiModel> raceResult)
        {
            RaceResult = raceResult;
        }

        public int CalculatePoints(List<ApiModel> team)
        {
            var drivers = team.Where(a => !a.is_constructor).ToList();

            int points = CalculateQualifying(drivers);
            points += CalculateRace(drivers);
            return points;
        }

        private int CalculateQualifying(List<ApiModel> drivers)
        {
            int points = CalculatePositionPoints(drivers, QualiPoints);
            points += CalcualteBeatTeammatePoints(drivers, QualiBeatTeammatePoints);
            return points;
        }

        private int CalculateRace(List<ApiModel> drivers)
        {
            //1 point for finishing the race
            int points = 1;
            points += CalculatePositionPoints(drivers, RacePoints);
            points += CalcualteBeatTeammatePoints(drivers, RaceBeatTeammatePoints);
            return points;
        }

        private int CalculatePositionPoints(List<ApiModel> drivers, List<int> pointsForPosition)
        {
            int points = 0;
            foreach (var driver in drivers)
            {
                int position = RaceResult.IndexOf(driver);
                if (position < 10)
                {
                    points += pointsForPosition[position];
                }
            }
            return points;
        }

        private int CalcualteBeatTeammatePoints(List<ApiModel> drivers, int pointsForbeatingTeammate)
        {
            int points = 0;
            foreach (var driver in drivers)
            {
                points += DidBeatTeammate(driver) ? pointsForbeatingTeammate : 0;
            }
            return points;
        }

        private bool DidBeatTeammate(ApiModel driver)
        {
            var teammate = RaceResult.Single(a => a.team_id == driver.team_id);
            return RaceResult.IndexOf(driver) < RaceResult.IndexOf(teammate);
        }
    }
}
