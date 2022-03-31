namespace F1FantasySim.Models
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