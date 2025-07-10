namespace Manager.Server.Models
{
    public class Team
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;

        public Team() { }

        public Team(int id, string name, string managerName)
        {
            Id = id;
            Name = name;
            ManagerName = managerName;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name} Manager: {ManagerName}";
        }
    }
}
