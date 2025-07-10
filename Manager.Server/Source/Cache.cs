using Manager.Server.Interfaces;
using Manager.Server.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Server.Source
{
    public class Cache
    {
        public GameWeek GameWeek { get; set; } = new();
        public List<Fixture> Fixtures { get; set; } = [];
        public Dictionary<int, TeamData> Teams { get; set; } = [];
        public Dictionary<int, Position> Positions { get; set; } = [];
        public Dictionary<int, PlayerData> Players { get; set; } = [];
        public List<UpcomingFixtures> AllUpcomingFixtures { get; set; } = [];

        private readonly ILogger<Cache> _logger;

        public Cache(ILogger<Cache> logger)
        {
            _logger = logger;
        }

        public void Initialise()
        {
            _logger.LogInformation("Initialising Cache");
            ProcessFixtures();
        }

        /// <summary>
        /// Generates a list of upcoming fixtures which includes the team
        /// and their difficulty
        /// </summary>
        private void ProcessFixtures()
        {
            AllUpcomingFixtures = FutureFixtureProcessor.ProcessFixtures(Fixtures, Teams, limit: 5);
        }

        public int Week => GameWeek.Id;
    }
}