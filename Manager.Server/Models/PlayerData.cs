namespace Manager.Server.Models
{
    public class PlayerData
    {
        public bool IsLive { get; set; } = false;
        public int Multiplier { get; set; } = 1;
        public int ImageCode { get; set; }
        public int BenchOrder { get; set; } = 0;

        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;

        public int? TeamId { get; set; } = null;
        public string TeamName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;
        public string PositionShort { get; set; } = string.Empty;

        public int WeekPoints { get; set; }
        public int TotalPoints { get; set; }
        public int BonusPoints { get; set; }

        public int Minutes { get; set; }
        public int GoalsScored { get; set; }
        public string ExpectedGoalsScored { get; set; } = string.Empty;
        public int Assists { get; set; }
        public int CleanSheets { get; set; }
        public int GoalsConceded { get; set; }
        public int OwnGoals { get; set; }
        public int Saves { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {FirstName} {SecondName}";
        }
    }
}
