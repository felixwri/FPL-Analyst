namespace Manager.Server.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Short { get; set; } = string.Empty;
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
