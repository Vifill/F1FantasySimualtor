using F1FantasySim.Models.NewAPI;
using F1FantasySim.Models.OldAPI;

namespace F1FantasySim.Models
{
    public class CompetitorViewModel
    {
        public CompetitorViewModel(DriverApiModel apiModel, PointsBreakdown points, bool isTurboed)
        {
            ApiModel = apiModel;
            Points = points;
            IsTurboed = isTurboed;
        }

        public DriverApiModel ApiModel { get; set; }
        public PointsBreakdown Points { get; set; }
        public bool IsTurboed { get; set; }
    }
}