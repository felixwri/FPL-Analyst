namespace Manager.Server.Models
{
    public class TeamData : Team
    {
        public int Code { get; set; }
        public int Strength_Overall_Home { get; set; }
        public int Strength_Overall_Away { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Strength H: {Strength_Overall_Home}, Strength_A: {Strength_Overall_Away}";
        }
    }

}
