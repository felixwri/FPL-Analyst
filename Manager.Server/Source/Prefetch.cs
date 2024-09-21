using Newtonsoft.Json.Linq;

namespace Manager.Server.Source
{
    public class Prefetch
    {
        public async Task<List<JToken>> UnpackBootstrapData(string key)
        {
            string bootstapString = await Fetch.Get(Resources.Bootstrap());

            JObject bootstapObject = JObject.Parse(bootstapString);

            JArray results = JArray.Parse(bootstapObject[key]?.ToString() ?? string.Empty);

            List<JToken> res = [.. results.Children()];

            return res;
        }

        /// <summary>
        /// Deserializes JSON from the Resources.Bootstrp API
        /// </summary>
        /// <returns>Dictionary with each teams ID as the key</returns>
        public async Task<Dictionary<int, TeamData>> GetTeamAssignment()
        {
            List<JToken> teams = await UnpackBootstrapData("teams");

            Dictionary<int, TeamData> teamDataList = [];
            foreach (JToken result in teams)
            {
                TeamData? teamData = result.ToObject<TeamData>();
                if (teamData != null)
                {
                    teamDataList.Add(teamData.Id, teamData);
                }
            }
            return teamDataList;
        }

        public async Task<GameWeek> GetStaticContent()
        {
            List<JToken> gameweeks = await UnpackBootstrapData("events");

            GameWeek gameWeek = new();

            JToken previous = gameweeks.First();

            foreach (JToken week in gameweeks)
            {
                bool isCurrent = week["is_current"]?.ToObject<bool>() ?? false;
                bool isNext = week["is_next"]?.ToObject<bool>() ?? false;
                bool finished = week["finished"]?.ToObject<bool>() ?? false;

                bool previousFinished = previous["finished"]?.ToObject<bool>() ?? false;

                DateTime? deadlineTime = week["deadline_time"]?.ToObject<DateTime>();
                DateTime currentTime = DateTime.Now;

                bool isActive = deadlineTime.HasValue && deadlineTime.Value < currentTime;

                if (isCurrent)
                {
                    gameWeek.Checked = DateTime.Now;
                    gameWeek.Id = week["id"]?.ToObject<int>() ?? 0;
                    gameWeek.IsActive = isActive && !finished;
                    gameWeek.CurrentGameWeek = isCurrent;
                    gameWeek.NextGameWeek = isNext;
                    gameWeek.PreviousGameWeek = week["is_previous"]?.ToObject<bool>() ?? false;
                    return gameWeek;
                }

                previous = week;
            }
            return gameWeek;
        }

        public async Task<Dictionary<int, PlayerData>> GetPlayerAssignment()
        {
            List<JToken> players = await UnpackBootstrapData("elements");

            Dictionary<int, PlayerData> playerDataAssignment = [];

            foreach (JToken result in players)
            {
                int teamId = result["team"]?.ToObject<int>() ?? 0;
                PlayerData playerData = new()
                {
                    IsLive = false,
                    Id = result["id"]?.ToObject<int>() ?? 0,
                    FirstName = result["first_name"]?.ToString() ?? string.Empty,
                    SecondName = result["second_name"]?.ToString() ?? string.Empty,
                    TeamId = teamId,
                    TeamName = Cache.Instance.Teams[teamId].Name,
                    TotalPoints = result["total_points"]?.ToObject<int>() ?? 0,
                    BonusPoints = result["bonus"]?.ToObject<int>() ?? 0,
                    Minutes = result["minutes"]?.ToObject<int>() ?? 0,
                    GoalsScored = result["goals_scored"]?.ToObject<int>() ?? 0,
                    ExpectedGoalsScored = result["expected_goals"]?.ToString() ?? string.Empty,
                };

                playerDataAssignment.Add(playerData.Id, playerData);

            }
            Console.WriteLine(playerDataAssignment[182].FirstName);
            return playerDataAssignment;
        }

        /// <summary>
        /// Deseralizes a list of all the games in the season
        /// </summary>
        /// <returns>A List of Fixture Objects</returns>
        public async Task<List<Fixture>> GetFixtures()
        {
            string fixturesString = await Fetch.Get(Resources.Fixtures());

            JArray fixtures = JArray.Parse(fixturesString);

            List<JToken> results = fixtures.Children().ToList();

            List<Fixture> fixtureList = new();
            foreach (JToken result in results)
            {
                Fixture? fixture = result.ToObject<Fixture>();
                if (fixture != null)
                {
                    fixtureList.Add(fixture);
                }
            }
            return fixtureList;
        }
    }
}

