using Newtonsoft.Json.Linq;

namespace Manager.Server
{
    public class Prefetch
    {
        /// <summary>
        /// Deserializes JSON from the Resources.Bootstrp API
        /// </summary>
        /// <returns>Dictionary with each teams ID as the key</returns>
        public async Task<Dictionary<int, TeamData>> GetTeamAssignment()
        {
            string bootstapString = await Fetch.Get(Resources.Bootstrap());

            JObject bootstapObject = JObject.Parse(bootstapString);

            JArray fixtures = JArray.Parse(bootstapObject["teams"]?.ToString() ?? string.Empty);

            List<JToken> results = fixtures.Children().ToList();

            Dictionary<int, TeamData> teamDataList = new();
            foreach (JToken result in results)
            {
                TeamData? teamData = result.ToObject<TeamData>();
                if (teamData != null)
                {
                    teamDataList.Add(teamData.Id, teamData);
                }
            }
            return teamDataList;
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

    public class TeamData
    {
        public int Code { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Strength_Overall_Home { get; set; } = string.Empty;
        public string Strength_Overall_Away { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Strength H: {Strength_Overall_Home}, Strength_A: {Strength_Overall_Away}";
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
}

