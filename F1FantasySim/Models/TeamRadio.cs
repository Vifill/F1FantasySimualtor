using System.Text.Json.Serialization;

namespace F1FantasySim.Models
{
    public class TeamRadio
    {
        [JsonPropertyName("session_key")]
        public int SessionKey { get; set; }

        [JsonPropertyName("meeting_key")]
        public int MeetingKey { get; set; }

        [JsonPropertyName("driver_number")]
        public int DriverNumber { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("recording_url")]
        public string RecordingUrl { get; set; }
    }

}
