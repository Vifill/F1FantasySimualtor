namespace F1FantasySim.Models
{
    public class CalculationTeamModel
    {
        public CalculationTeamModel(List<ApiModel> drivers, ApiModel constructor, int points, double price)
        {
            Drivers = drivers;
            Constructor = constructor;
            Points = points;
            Price = price;
        }

        public List<ApiModel> Drivers { get; set; }
        public ApiModel Constructor { get; set; }
        public int Points { get; set; }
        public double Price { get; set; }
    }
}
