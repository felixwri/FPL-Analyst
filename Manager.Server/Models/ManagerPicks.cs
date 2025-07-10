namespace Manager.Server.Models
{
    public class ManagerPicks : Team
    {
        public bool IsLive { get; set; } = false;
        public int Gameweek { get; set; }
        public List<PlayerData> Picks { get; set; } = [];
    }
}
