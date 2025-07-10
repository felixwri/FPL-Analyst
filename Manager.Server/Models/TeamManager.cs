namespace Manager.Server.Models
{
    public class TeamManager : Team
    {
        public int TotalPoints { get; set; } = 0;
        public int GameweekPoints { get; set; } = 0;

        public int Rank { get; set; } = 0;
        public int LastRank { get; set; } = 0;
    }
}
