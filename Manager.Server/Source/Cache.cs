namespace Manager.Server.Source
{
    public class Cache
    {
        private static readonly Cache instance = new();

        public GameWeek GameWeek = new();
        public List<Fixture> Fixtures = [];
        public Dictionary<int, TeamData> Teams = [];
        public Dictionary<int, PlayerData> Players = [];
        public List<UpcomingFixtures> AllUpcomingFixtures = [];

        private Cache()
        {
            Console.WriteLine("Pre-Processing");
            Load();
        }

        private async void Load()
        {
            Prefetch prefetch = new();
            GameWeek = await prefetch.GetStaticContent();
            Teams = await prefetch.GetTeamAssignment();
            Fixtures = await prefetch.GetFixtures();
            Players = await prefetch.GetPlayerAssignment();

            ProcessFixtures();
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

        public int Week
        {
            get { return GameWeek.Id; }
        }
    }
}
