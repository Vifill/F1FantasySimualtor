namespace F1FantasySim.Models.NewAPI
{
    public class PointsBreakdown
    {
        public int QualifyingPoints { get; set; }
        public int RacePoints { get; set; }
        public int OvertakingPoints { get; set; }
        public bool IsTurboed { get; set; } = false;
        public int TotalPoints => (QualifyingPoints + RacePoints + OvertakingPoints) * (IsTurboed ? 2 : 1);

        public PointsBreakdown()
        {
        }

        public PointsBreakdown(int qualifyingPoints, int racePoints)
        {
            QualifyingPoints = qualifyingPoints;
            RacePoints = racePoints;
            OvertakingPoints = 0;
        }

        // Copy constructor
        public PointsBreakdown(PointsBreakdown other)
        {
            if (other != null)
            {
                QualifyingPoints = other.QualifyingPoints;
                RacePoints = other.RacePoints;
                OvertakingPoints = other.OvertakingPoints;
                IsTurboed = other.IsTurboed;
            }
        }

        public void Aggregate(PointsBreakdown other)
        {
            if (other != null)
            {
                QualifyingPoints += other.QualifyingPoints;
                RacePoints += other.RacePoints;
                OvertakingPoints += other.OvertakingPoints;
                // Note: IsTurboed is not aggregated because it's a property that should be explicitly set
            }
        }
    }
}
