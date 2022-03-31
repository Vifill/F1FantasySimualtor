namespace F1FantasySim.Models
{
    public class CalculationTeamModel
    {
        public CalculationTeamModel(List<CompetitorViewModel> drivers, CompetitorViewModel constructor, int points, double price)
        {
            Drivers = drivers;
            Constructor = constructor;
            Points = points;
            Price = price;
        }

        public List<CompetitorViewModel> Drivers { get; set; }
        public CompetitorViewModel Constructor { get; set; }
        public int Points { get; set; }
        public double Price { get; set; }
    }
}
