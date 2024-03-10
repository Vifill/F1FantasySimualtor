using System.Globalization;

namespace F1FantasySim.Models.NewAPI
{
	public class PlayerApiModel
	{
		public string PlayerId { get; set; }
		public int Skill { get; set; }
		public string PositionName { get; set; }
		public double Value { get; set; }
		public string TeamId { get; set; }
		public string FullName { get; set; }
		public string DisplayName { get; set; }
		public string TeamName { get; set; }
		public string Status { get; set; }
		public string IsActive { get; set; }
		public string DriverTLA { get; set; }
		public string CountryName { get; set; }
		public string OverallPpints { get; set; }
		public string GamedayPoints { get; set; }
		public string SelectedPercentage { get; set; }
		public string CaptainSelectedPercentage { get; set; }
		public double OldPlayerValue { get; set; }
		public string BestRaceFinished { get; set; }
		public string HighestGridStart { get; set; }
		public string HighestChampFinish { get; set; }
		public string FastestPitstopAward { get; set; }
		public int BestRaceFinishCount { get; set; }
		public int HighestGridStartCount { get; set; }
		public int HighestChampFinishCount { get; set; }
		public int FastestPitstopAwardCount { get; set; }
		// Add other properties as needed
		public string F1PlayerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public double PointsPerMill { get; private set; }

		public double AveragePointsPerRace(int numOfraces)
		{
			return double.Parse(OverallPpints, CultureInfo.InvariantCulture) / numOfraces;
		}

		public double PointsPerMillion(int numOfRaces)
		{
            PointsPerMill = Math.Round((AveragePointsPerRace(numOfRaces) / Value), 2);
			return PointsPerMill;
		}

		public bool IsConstructor()
		{
			return PositionName.Equals("CONSTRUCTOR", StringComparison.InvariantCultureIgnoreCase);
		}
    }
}
