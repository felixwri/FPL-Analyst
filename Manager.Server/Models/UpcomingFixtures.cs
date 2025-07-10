using Manager.Server.Source;

namespace Manager.Server.Models
{
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
