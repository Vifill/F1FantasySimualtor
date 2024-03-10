namespace F1FantasySim.Models
{
    public class CalculationTeamModel
    {
        public CalculationTeamModel(List<CompetitorViewModel> drivers, List<CompetitorViewModel> constructors, int points, double price)
        {
            Drivers = drivers;
            Constructors = constructors;
            Points = points;
            Price = price;
        }

        public List<CompetitorViewModel> Drivers { get; set; }
        public List<CompetitorViewModel> Constructors { get; set; }
        public int Points { get; set; }
        public double Price { get; set; }
    }
}
