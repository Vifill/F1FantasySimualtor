namespace F1FantasySim.Models.NewAPI.LeagueView
{
    public class LeagueApiModel
    {
        public LeagueInfo leagueInfo { get; set; }
        public List<MemRank> memRank { get; set; }
        public List<UserRank> userRank { get; set; }
    }

    public class LeagueInfo
    {
        public int leagueid { get; set; }
        public string memCount { get; set; }
        public string leagueCode { get; set; }
        public string leagueName { get; set; }
    }

    public class MemRank
    {
        public int teamId { get; set; }
        public int teamNo { get; set; }
        public string teamName { get; set; }
        public string userName { get; set; }
        public string guid { get; set; }
        public int isAdmin { get; set; }
        public int rno { get; set; }
        public int ovPoints { get; set; }
        public int rank { get; set; }
        public int trend { get; set; }
        public int lpgdid { get; set; }
    }

    public class UserRank
    {
        public int teamId { get; set; }
        public int teamNo { get; set; }
        public string teamName { get; set; }
        public string userName { get; set; }
        public string guid { get; set; }
        public int isAdmin { get; set; }
        public int rno { get; set; }
        public int ovPoints { get; set; }
        public int rank { get; set; }
        public int trend { get; set; }
    }
}
