using F1FantasySim.Models.OldAPI;

namespace F1FantasySim.Models.NewAPI.LeagueView
{
    public class PlayerLeagueApiModels
    {
    }

    public class Player
    {
        public string id { get; set; }
        public int isfinal { get; set; }
        public int iscaptain { get; set; }
        public int ismgcaptain { get; set; }
        public int playerpostion { get; set; }

        public DriverApiModel DriverDetails { get; set; }
    }

    public class TeamInfo
    {
        public TeamInfo()
        {
        }

        public double? teamBal { get; set; }
        public double? teamVal { get; set; }
        public double? maxTeambal { get; set; }
        public int? subsallowed { get; set; }
        public int? userSubsleft { get; set; }
    }

    public class UserTeam
    {
        public int? gdrank { get; set; }
        public int? ovrank { get; set; }
        public int? teamno { get; set; }
        public double? teambal { get; set; }
        public double? teamval { get; set; }
        public int? gdpoints { get; set; }
        public int? matchday { get; set; }
        public double? ovpoints { get; set; }
        public List<Player> playerid { get; set; }
        public string teamname { get; set; }
        public int? usersubs { get; set; }
        public int? boosterid { get; set; }
        public TeamInfo team_info { get; set; }
        public int? fttourgdid { get; set; }
        public int? fttourmdid { get; set; }
        public int? iswildcard { get; set; }
        public double? maxteambal { get; set; }
        public string capplayerid { get; set; }
        public int? subsallowed { get; set; }
        public int? isaccounting { get; set; }
        public int? usersubsleft { get; set; }
        public int? extrasubscost { get; set; }
        public int? islateonboard { get; set; }
        public int? mgcapplayerid { get; set; }
        public string race_category { get; set; }
        public string? finalfxracecat { get; set; }
        public int? finalfxraceday { get; set; }
        public int? isboostertaken { get; set; }
        public int? extradrstakengd { get; set; }
        public int? finalfixtakengd { get; set; }
        public int? isextradrstaken { get; set; }
        public int? isfinalfixtaken { get; set; }
        public int? iswildcardtaken { get; set; }
        public int? wildcardtakengd { get; set; }
        public int? autopilottakengd { get; set; }
        public int? isautopilottaken { get; set; }
        public int? islimitlesstaken { get; set; }
        public int? limitlesstakengd { get; set; }
        public int? isnonigativetaken { get; set; }
        public int? nonigativetakengd { get; set; }
        public int? finalfxnewplayerid { get; set; }
        public int? finalfxoldplayerid { get; set; }
        public int? is_wildcard_taken_gd_id { get; set; }
    }

    public class PlayerDetails
    {
        public int mdid { get; set; }
        public List<UserTeam> userTeam { get; set; }
        public int retval { get; set; }
    }
}
