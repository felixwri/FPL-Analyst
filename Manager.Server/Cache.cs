namespace Manager.Server
{
    public class Cache
    {
        private static readonly Cache instance = new();
        public List<Fixture> Fixtures = [];
        public Dictionary<int, TeamData> Teams = [];
        public List<UpcomingFixtures> AllUpcomingFixtures = [];

        private Cache()
        {
            Console.WriteLine("Pre-Processing");
            ProcessData();
        }

        /// <summary>
        /// Generates a list of upcoming fixtures which includes the team
        /// and their difficulty
        /// </summary>
        private async void ProcessData()
        {
            Prefetch prefetch = new();
            Teams = await prefetch.GetTeamAssignment();
            Fixtures = await prefetch.GetFixtures();

            foreach ((int Id, TeamData team) in Teams)
            {
                int n = 0;
                UpcomingFixtures upcomingFixtures = new()
                {
                    Team = team.Name,
                    Id = team.Id
                };

                foreach (Fixture fixture in Fixtures)
                {
                    if (n >= 5) break;
                    if (fixture.Finished) continue;
                    if (team.Id != fixture.Team_H && team.Id != fixture.Team_A) continue;
                    if (team.Id == fixture.Team_H)
                    {
                        FutureFixture futureFixture = new();

                        bool teamExists = Teams.TryGetValue(fixture.Team_A, out TeamData? awayTeam);

                        if (!teamExists) continue;

                        if (awayTeam != null)
                        {
                            futureFixture.Id = awayTeam.Id;
                            futureFixture.AtHome = true;
                            futureFixture.Opponent = awayTeam.Name;
                            futureFixture.OpponentDifficulty = awayTeam.Strength_Overall_Home;
                            futureFixture.Kickoff = fixture.Kickoff_Time;
                            upcomingFixtures.Fixtures.Add(futureFixture);
                        }
                        n++;
                    }

                    else
                    {
                        FutureFixture futureFixture = new();

                        bool teamExists = Teams.TryGetValue(fixture.Team_H, out TeamData? homeTeam);

                        if (!teamExists) continue;

                        if (homeTeam != null)
                        {
                            futureFixture.Id = homeTeam.Id;
                            futureFixture.AtHome = false;
                            futureFixture.Opponent = homeTeam.Name;
                            futureFixture.OpponentDifficulty = homeTeam.Strength_Overall_Home;
                            futureFixture.Kickoff = fixture.Kickoff_Time;

                            upcomingFixtures.Fixtures.Add(futureFixture);
                        }

                        n++;
                    }
                }
                AllUpcomingFixtures.Add(upcomingFixtures);
            }
        }

        public static Cache Instance
        {
            get { return instance; }
        }
    }

    /// <summary>
    /// All upcoming fixtures for a team
    /// </summary>
    public class UpcomingFixtures
    {
        public int Id { get; set; }
        public string Team { get; set; } = string.Empty;
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
        public string OpponentDifficulty { get; set; } = string.Empty;
        public DateTime Kickoff { get; set; }
    }
}
