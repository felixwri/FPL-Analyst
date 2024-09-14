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
            ProcessData();
        }

        /// <summary>
        /// Generates a list of upcoming fixtures which includes the team
        /// and their difficulty
        /// </summary>
        private async void ProcessData(int limit = 5)
        {
            Prefetch prefetch = new();
            Teams = await prefetch.GetTeamAssignment();
            Fixtures = await prefetch.GetFixtures();
            Players = await prefetch.GetPlayerAssignment();


            foreach ((int _, TeamData team) in Teams)
            {
                int n = 0;
                UpcomingFixtures upcomingFixtures = new()
                {
                    Team = team.Name,
                    Id = team.Id,
                    TeamDifficultyHome = team.Strength_Overall_Home,
                    TeamDifficultyAway = team.Strength_Overall_Away,
                };

                foreach (Fixture fixture in Fixtures)
                {
                    if (n >= limit) break;
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

                            int difficulty = awayTeam.Strength_Overall_Away - upcomingFixtures.TeamDifficultyHome / 4;

                            futureFixture.RelativeDifficulty = difficulty;
                            futureFixture.OpponentDifficulty = awayTeam.Strength_Overall_Away;
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

                            int difficulty = homeTeam.Strength_Overall_Away - upcomingFixtures.TeamDifficultyAway / 4;

                            futureFixture.RelativeDifficulty = difficulty;
                            futureFixture.OpponentDifficulty = homeTeam.Strength_Overall_Away;
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
}
