using System.Text.Json;

namespace Manager.Server.Source
{
    public class JsonOptionsProvider
    {
        public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
    }

    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Short { get; set; } = string.Empty;
        public int Min { get; set; }
        public int Max { get; set; }
    }


    public class GameWeek
    {
        public DateTime Checked { get; set; } = DateTime.Now;
        public int Id { get; set; } = 1;
        public bool IsActive { get; set; } = false;
        public bool CurrentGameWeek { get; set; } = true;
        public bool NextGameWeek { get; set; } = false;
        public bool PreviousGameWeek { get; set; } = false;
    }

    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Week { get; set; } = 1;
        public int Size { get; set; } = 0;
        public List<TeamManager> Teams { get; set; } = [];
    }

    public class Team
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

        public Team() { }

        public Team(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class TeamManager : Team
    {
        public string ManagerName { get; set; } = string.Empty;

        public int TotalPoints { get; set; } = 0;
        public int GameweekPoints { get; set; } = 0;

        public int Rank { get; set; } = 0;
        public int LastRank { get; set; } = 0;
    }

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


    public class ManagerPicks
    {
        public bool IsLive { get; set; } = false;
        public int Gameweek { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<PlayerData> Picks { get; set; } = [];
    }

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