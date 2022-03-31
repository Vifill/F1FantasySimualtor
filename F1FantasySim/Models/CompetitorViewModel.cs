namespace F1FantasySim.Models
{
    public class CompetitorViewModel
    {
        public CompetitorViewModel(ApiModel apiModel, int points, bool isTurboed)
        {
            ApiModel = apiModel;
            Points = points;
            IsTurboed = isTurboed;
        }

        public ApiModel ApiModel { get; set; }
        public int Points { get; set; }
        public bool IsTurboed { get; set; }
    }
}