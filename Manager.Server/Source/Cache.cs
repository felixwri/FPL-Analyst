namespace Manager.Server.Source
{
    public class Cache
    {
        private static readonly Cache instance = new();
        public List<Fixture> Fixtures = [];
        public Dictionary<int, TeamData> Teams = [];
        public Dictionary<int, PlayerData> Players = [];
        public List<UpcomingFixtures> AllUpcomingFixtures = [];

        private Cache()
        {
            Console.WriteLine("Pre-Processing");
            Load();
            ProcessFixtures();
        }

        private async void Load()
        {
            Prefetch prefetch = new();
            Teams = await prefetch.GetTeamAssignment();
            Fixtures = await prefetch.GetFixtures();
            Players = await prefetch.GetPlayerAssignment();
        }

        /// <summary>
        /// Generates a list of upcoming fixtures which includes the team
        /// and their difficulty
        /// </summary>
        private void ProcessFixtures()
        {
            AllUpcomingFixtures = PreProcessing.ProcessFixtures(Fixtures, Teams, limit: 5);
        }

        public static Cache Instance
        {
            get { return instance; }
        }
    }
}
