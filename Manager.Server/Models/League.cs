using Manager.Server.Source;

namespace Manager.Server.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Week { get; set; } = 1;
        public int Size { get; set; } = 0;
        public List<TeamManager> Teams { get; set; } = [];
    }
}
