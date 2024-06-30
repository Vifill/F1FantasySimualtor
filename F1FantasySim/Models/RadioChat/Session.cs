using System.Text.Json.Serialization;

namespace F1FantasySim.Models.RadioChat
{
    public class Session
    {
        [JsonPropertyName("session_key")]
        public int SessionKey { get; set; }

        [JsonPropertyName("session_name")]
        public string SessionName { get; set; }

        [JsonPropertyName("date_start")]
        public DateTime DateStart { get; set; }

        [JsonPropertyName("date_end")]
        public DateTime DateEnd { get; set; }

        [JsonPropertyName("gmt_offset")]
        public string GmtOffset { get; set; }

        [JsonPropertyName("session_type")]
        public string SessionType { get; set; }

        [JsonPropertyName("meeting_key")]
        public int MeetingKey { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("country_key")]
        public int CountryKey { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("circuit_key")]
        public int CircuitKey { get; set; }

        [JsonPropertyName("circuit_short_name")]
        public string CircuitShortName { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }
    }
}
