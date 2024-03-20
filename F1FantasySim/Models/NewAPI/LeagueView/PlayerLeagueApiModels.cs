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
        public double? gdpoints { get; set; }
        public int? matchday { get; set; }
        public double? ovpoints { get; set; }
        public List<Player> playerid { get; set; }
        public string teamname { get; set; }
        public int? usersubs { get; set; }
        public int? boosterid { get; set; }
        public TeamInfo team_info { get; set; }
    }

    public class PlayerDetails
    {
        public int mdid { get; set; }
        public List<UserTeam> userTeam { get; set; }
        public int retval { get; set; }
    }
}
