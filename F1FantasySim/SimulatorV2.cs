using System.Diagnostics;
using System.Linq;
using F1FantasySim.Models;
using F1FantasySim.Models.NewAPI;
using F1FantasySim.Models.OldAPI;

namespace F1FantasySim
{
    public class SimulatorV2
    {
        public List<DriverApiModel> RaceResult;
        public List<DriverApiModel> QualiResult;
        public List<DriverApiModel> Constructors;

        private readonly List<int> QualiPoints = new List<int>() { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        private readonly List<int> RacePoints = new List<int>() { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };

        private static Dictionary<DriverApiModel, PointsBreakdown> AllPlayerPoints;

        public SimulatorV2(List<DriverApiModel> raceResult, List<DriverApiModel> qualiResult, List<DriverApiModel> constructors)
        {
            RaceResult = raceResult;
            QualiResult = qualiResult;
            Constructors = constructors;
            AllPlayerPoints = new Dictionary<DriverApiModel, PointsBreakdown>();
        }

        public List<CompetitorViewModel> GetAllPlayerPoints()
        {
            var allPoints = CalculatePoints();
            return allPoints.Select(a => new CompetitorViewModel(a.Key, a.Value, false)).ToList();
        }

        public List<CompetitorViewModel> CalculatePoints(List<DriverApiModel> team)
        {
            if (AllPlayerPoints.Count == 0)
            {
                AllPlayerPoints = CalculatePoints();
            }

            // Filter out the constructors from the team
            var constructors = team.Where(a => a.IsConstructor()).ToList();

            // Calculate points for drivers
            var playerPoints = AllPlayerPoints
                .Where(a => team.Any(b => b.PlayerId == a.Key.PlayerId) && !a.Key.IsConstructor())
                .Select(a => new CompetitorViewModel(a.Key, a.Value, false))
                .OrderByDescending(a => a.Points.TotalPoints)
                .ToList();

            // Calculate points for each constructor and add to the list
            //var constructorPoints = AllPlayerPoints.Where(a=> a.)
            //playerPoints.AddRange(constructorPoints);

            // Set turbo driver (assuming the first non-constructor is always chosen, this logic might need adjusting)
            var turboDriver = playerPoints.FirstOrDefault(a => !a.ApiModel.IsConstructor());
            if (turboDriver != null)
            {
                turboDriver.Points.IsTurboed = true;
                turboDriver.IsTurboed = true;
            }

            return playerPoints;
        }

        private Dictionary<DriverApiModel, PointsBreakdown> CalculatePoints()
        {
            var allPlayerPoints = RaceResult
                .Where(a => !a.IsConstructor())
                .ToDictionary(driver => driver, driver => new PointsBreakdown());

            CalculateQualifying(allPlayerPoints);
            CalculateRace(allPlayerPoints);

            // Add constructors to allPlayerPoints with empty PointsBreakdown initially
            //foreach (var constructor in Constructors)
            //{
            //    if(!allPlayerPoints.ContainsKey(constructor))
            //    {
            //        allPlayerPoints.Add(constructor, new PointsBreakdown());
            //    }
            //}

            // Now calculate points for constructors based on their drivers' points
            CalculateConstructorPoints(allPlayerPoints);

            return allPlayerPoints;
        }

        private void CalculateQualifying(Dictionary<DriverApiModel, PointsBreakdown> allDriverPoints)
        {
            // Calculate qualifying points for drivers
            CalculatePositionPoints(allDriverPoints, QualiPoints, (breakdown, points) => breakdown.QualifyingPoints += points);

            // Calculate qualifying points for constructors based on drivers' performance
            var constructors = allDriverPoints.Keys.Where(player => player.IsConstructor()).ToList();
            //CalculateConstructorQualifyingPoints(allDriverPoints);
        }

        private void CalculateConstructorQualifyingPoints(Dictionary<DriverApiModel, PointsBreakdown> allDriverPoints)
        {
            foreach (var constructor in Constructors)
            {
                var constructorDrivers = allDriverPoints.Keys.Where(driver => driver.TeamId == constructor.TeamId).ToList();

                int driversInQ2 = constructorDrivers.Count(driver => QualiResult.IndexOf(driver) < 15); // assuming Q2 is the top 15
                int driversInQ3 = constructorDrivers.Count(driver => QualiResult.IndexOf(driver) < 10); // assuming Q3 is the top 10

                var constructorPoints = new PointsBreakdown();

                // Apply rules based on the number of drivers in Q2 and Q3
                if (driversInQ2 == 0)
                {
                    constructorPoints.QualifyingPoints -= 1;
                }
                else if (driversInQ2 == 1)
                {
                    constructorPoints.QualifyingPoints += 1;
                }
                else if (driversInQ2 == 2)
                {
                    constructorPoints.QualifyingPoints += 3;
                }

                if (driversInQ3 == 1)
                {
                    constructorPoints.QualifyingPoints += 5;
                }
                else if (driversInQ3 == 2)
                {
                    constructorPoints.QualifyingPoints += 10;
                }

                allDriverPoints[constructor] = constructorPoints;
            }
        }


        private void CalculateRace(Dictionary<DriverApiModel, PointsBreakdown> drivers)
        {
            // Calculate standard race points
            CalculatePositionPoints(drivers, RacePoints, (breakdown, points) => breakdown.RacePoints += points );

            // Calculate additional points for positions gained during the race
            CalculateGainedPositionPoints(drivers);
        }


        private void CalculateGainedPositionPoints(Dictionary<DriverApiModel, PointsBreakdown> drivers)
        {
            foreach (var driver in RaceResult)
            {
                if (!driver.IsConstructor())
                {
                    // Find the qualifying position of the driver
                    int qualiPosition = QualiResult.IndexOf(driver) + 1; // Positions are 1-indexed
                                                                         // Find the race position of the driver
                    int racePosition = RaceResult.IndexOf(driver) + 1;

                    // If the driver finished the race higher than their qualifying position, award points
                    if (racePosition < qualiPosition)
                    {
                        int positionsGained = qualiPosition - racePosition;
                        int gainedPoints = positionsGained * 2; // 2 points per position gained
                        drivers[driver].OvertakingPoints += gainedPoints; // Add the gained points to the driver's total
                    }
                }
            }
        }

        private void CalculatePositionPoints(
            Dictionary<DriverApiModel, PointsBreakdown> drivers,
            List<int> pointsForPosition,
            Action<PointsBreakdown, int> updatePointsAction)
        {
            foreach (var driver in drivers.Keys)
            {
                int position = RaceResult.IndexOf(driver);
                if (position >= 0 && position < pointsForPosition.Count)
                {
                    // Use the passed lambda expression to update the points
                    updatePointsAction(drivers[driver], pointsForPosition[position]);
                }
            }
        }


        private void CalculateConstructorPoints(Dictionary<DriverApiModel, PointsBreakdown> allPlayerPoints)
        {
            CalculateConstructorQualifyingPoints(allPlayerPoints);
            CalculateConstructorRacePoints(allPlayerPoints);

            //var constructorPointsList = new List<CompetitorViewModel>();

            //foreach (var constructor in constructors)
            //{
            //    var constructorPointsBreakdown = new PointsBreakdown();

            //    // Aggregate points from drivers
            //    foreach (var driverPointsKvp in allPlayerPoints)
            //    {
            //        if (driverPointsKvp.Key.TeamId == constructor.TeamId)
            //        {
            //            constructorPointsBreakdown.Aggregate(driverPointsKvp.Value);
            //        }
            //    }

            //    constructorPointsList.Add(new CompetitorViewModel(constructor, constructorPointsBreakdown, false));
            //}

            //return constructorPointsList;
        }

        private void CalculateConstructorRacePoints(Dictionary<DriverApiModel, PointsBreakdown> allPlayerPoints)
        {
            foreach (var constructor in Constructors)
            {
                var constructorPointsBreakdown = allPlayerPoints[constructor];

                //TODO:  This is not right, need to think how constructor points are calculated
                // Aggregate points from drivers
                foreach (var driverPointsKvp in allPlayerPoints)
                {
                    if (driverPointsKvp.Key.TeamId == constructor.TeamId)
                    {
                        constructorPointsBreakdown.Aggregate(driverPointsKvp.Value);
                    }
                }
                //allPlayerPoints[constructor] = 
                allPlayerPoints.Add(constructor, constructorPointsBreakdown);
            }
        }
    }
}
