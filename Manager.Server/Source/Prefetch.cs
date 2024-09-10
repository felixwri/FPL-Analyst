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

        public async Task<Dictionary<int, PlayerData>> GetPlayerAssignment()
        {
            List<JToken> players = await UnpackBootstrapData("elements");

            Dictionary<int, PlayerData> playerDataList = [];

            foreach (JToken result in players)
            {
                PlayerData? playerData = result.ToObject<PlayerData>();
                if (playerData != null)
                {
                    playerDataList.Add(playerData.Id, playerData);
                }
            }
            return playerDataList;
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

