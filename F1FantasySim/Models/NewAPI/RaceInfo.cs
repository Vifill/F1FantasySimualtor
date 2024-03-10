using Newtonsoft.Json;

namespace F1FantasySim.Models.NewAPI
{
	public class RaceInfo
	{
		[JsonProperty("RaceId")]
		public int RaceId { get; set; }

		[JsonProperty("GamedayId")]
		public int GamedayId { get; set; }

		[JsonProperty("SessionName")]
		public string SessionName { get; set; }

		[JsonProperty("SessionStartDate")]
		public string SessionStartDate { get; set; }

		[JsonProperty("GDStatus")]
		public int GDStatus { get; set; }

		// Add other properties as needed
	}
}
