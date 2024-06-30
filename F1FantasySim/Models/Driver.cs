﻿using System.Text.Json.Serialization;

namespace F1FantasySim.Models
{
    public class Driver
    {
        [JsonPropertyName("session_key")]
        public int SessionKey { get; set; }

        [JsonPropertyName("meeting_key")]
        public int MeetingKey { get; set; }

        [JsonPropertyName("broadcast_name")]
        public string BroadcastName { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("headshot_url")]
        public string HeadshotUrl { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("driver_number")]
        public int DriverNumber { get; set; }

        [JsonPropertyName("team_colour")]
        public string TeamColour { get; set; }

        [JsonPropertyName("team_name")]
        public string TeamName { get; set; }

        [JsonPropertyName("name_acronym")]
        public string NameAcronym { get; set; }
    }

}
