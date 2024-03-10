using F1FantasySim.Models.NewAPI;
using F1FantasySim.Models.OldAPI;

namespace F1FantasySim.Models
{
    public class CompetitorViewModel
    {
        public CompetitorViewModel(PlayerApiModel apiModel, PointsBreakdown points, bool isTurboed)
        {
            ApiModel = apiModel;
            Points = points;
            IsTurboed = isTurboed;
        }

        public PlayerApiModel ApiModel { get; set; }
        public PointsBreakdown Points { get; set; }
        public bool IsTurboed { get; set; }
    }
}