namespace F1FantasySim.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Players
    {
        public List<ApiModel> players { get; set; }
    }

    public class ApiModel
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string team_name { get; set; }
        public string position { get; set; }
        public int position_id { get; set; }
        public string position_abbreviation { get; set; }
        public double price { get; set; }
        public CurrentPriceChangeInfo current_price_change_info { get; set; }
        public object status { get; set; }
        public bool injured { get; set; }
        public object injury_type { get; set; }
        public bool banned { get; set; }
        public object ban_type { get; set; }
        public StreakEventsProgress streak_events_progress { get; set; }
        public double chance_of_playing { get; set; }
        public string team_abbreviation { get; set; }
        public double weekly_price_change { get; set; }
        public int weekly_price_change_percentage { get; set; }
        public int team_id { get; set; }
        public Headshot headshot { get; set; }
        public string known_name { get; set; }
        public JerseyImage jersey_image { get; set; }
        public int score { get; set; }
        public object humanize_status { get; set; }
        public object shirt_number { get; set; }
        public object country { get; set; }
        public object country_iso { get; set; }
        public bool is_constructor { get; set; }
        public double season_score { get; set; }
        public object driver_data { get; set; }
        public ConstructorData constructor_data { get; set; }
        public object born_at { get; set; }
        public List<SeasonPrice> season_prices { get; set; }
        public int num_fixtures_in_gameweek { get; set; }
        public bool deleted_in_feed { get; set; }
        public bool has_fixture { get; set; }
        public string display_name { get; set; }
        public string external_id { get; set; }
        public ProfileImage profile_image { get; set; }
        public MiscImage misc_image { get; set; }
    }

    public class CurrentPriceChangeInfo
    {
        public double current_selection_percentage { get; set; }
        public double probability_price_up_percentage { get; set; }
        public double probability_price_down_percentage { get; set; }
    }

    public class StreakEventsProgress
    {
        public string top_ten_in_a_row_qualifying_progress { get; set; }
        public string top_ten_in_a_row_race_progress { get; set; }
    }

    public class Headshot
    {
        public string profile { get; set; }
        public string pitch_view { get; set; }
        public string player_list { get; set; }
    }

    public class JerseyImage
    {
        public object url { get; set; }
    }

    public class ConstructorData
    {
        public int best_finish { get; set; }
        public int best_finish_count { get; set; }
        public int best_grid { get; set; }
        public int best_grid_count { get; set; }
        public int titles { get; set; }
        public double championship_points { get; set; }
        public string first_season { get; set; }
        public int poles { get; set; }
        public int fastest_laps { get; set; }
        public string country { get; set; }
        public string highest_race_finished { get; set; }
    }

    public class SeasonPrice
    {
        public int game_period_id { get; set; }
        public double price { get; set; }
    }

    public class ProfileImage
    {
        public string url { get; set; }
    }

    public class MiscImage
    {
        public object url { get; set; }
    }

    
}
