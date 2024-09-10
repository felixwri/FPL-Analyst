namespace Manager.Server.Source
{
    public class TeamData
    {
        public int Code { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Strength_Overall_Home { get; set; }
        public int Strength_Overall_Away { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Strength H: {Strength_Overall_Home}, Strength_A: {Strength_Overall_Away}";
        }
    }

    public class PlayerData
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;

        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;

        public int TotalPoints { get; set; }
        public int BonusPoints { get; set; }

        public int Minutes { get; set; }
        public int GoalsScored { get; set; }
        public int ExpectedGoalsScored { get; set; }
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

    public class Fixture
    {
        public int Code { get; set; }
        public int Id { get; set; }
        public int Team_H { get; set; }
        public int Team_A { get; set; }
        public bool Finished { get; set; }
        public DateTime Kickoff_Time { get; set; }

        public override string ToString()
        {
            return $"Team_a: {Team_A}, Team_H: {Team_H}, Finished: {Finished}";
        }
    }

    /// <summary>
    /// All upcoming fixtures for a team
    /// </summary>
    public class UpcomingFixtures
    {
        public int Id { get; set; }
        public string Team { get; set; } = string.Empty;
        public int TeamDifficultyHome { get; set; }
        public int TeamDifficultyAway { get; set; }
        public List<FutureFixture> Fixtures { get; set; } = [];
    }

    /// <summary>
    /// Data structure for a fixture with an opponent
    /// </summary>
    public class FutureFixture
    {
        public int Id { get; set; }
        public bool AtHome { get; set; }
        public string Opponent { get; set; } = string.Empty;
        public int OpponentDifficulty { get; set; }
        public int RelativeDifficulty { get; set; }
        public DateTime Kickoff { get; set; }
    }
}