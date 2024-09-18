namespace Manager.Server.Source
{
    public class PreProcessing
    {
        public static List<UpcomingFixtures> ProcessFixtures(
            List<Fixture> Fixtures,
            Dictionary<int, TeamData> Teams,
            int limit = 5
            )
        {
            List<UpcomingFixtures> AllUpcomingFixtures = [];

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
            return AllUpcomingFixtures;
        }
    }
}