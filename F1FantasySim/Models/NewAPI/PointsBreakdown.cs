namespace F1FantasySim.Models.NewAPI
{
    public class PointsBreakdown
    {
        public int QualifyingPoints { get; set; }
        public int RacePoints { get; set; }
        public int OvertakingPoints { get; set; }
        public bool IsTurboed { get; set; }
        public int TotalPoints => (QualifyingPoints + RacePoints + OvertakingPoints) * (IsTurboed ? 2 : 1);

        public PointsBreakdown()
        {
        }

        // Constructor
        public PointsBreakdown(int qualifyingPoints, int racePoints)
        {
            QualifyingPoints = qualifyingPoints;
            RacePoints = racePoints;
            OvertakingPoints = 0;
        }

        public void Aggregate(PointsBreakdown other)
        {
            QualifyingPoints += other.QualifyingPoints;
            RacePoints += other.RacePoints;
            OvertakingPoints += other.OvertakingPoints;
        }
    }
}
