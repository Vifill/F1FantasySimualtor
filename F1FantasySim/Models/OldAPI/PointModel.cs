namespace F1FantasySim.Models.OldAPI
{
    public class PointModel
    {
        public PointModel(int points, bool isTurboed)
        {
            Points = points;
            IsTurboed = isTurboed;
        }

        public int Points { get; set; }
        public bool IsTurboed { get; set; }
    }
}