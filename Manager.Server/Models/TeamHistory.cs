namespace Manager.Server.Models
{
    public class TeamHistory(int Id, string Name) : Team
    {
        public new int Id { get; set; } = Id;
        public new string Name { get; set; } = Name;
        public List<int> Points { get; set; } = [];
        public List<int> Total_points { get; set; } = [];
        public List<int> Points_on_bench { get; set; } = [];

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}";
        }
    }
}
