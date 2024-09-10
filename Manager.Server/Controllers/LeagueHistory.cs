using System.Text.Json;
using Manager.Server.Source;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;


namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeagueHistory : ControllerBase
    {
        private async Task<string> CompileLeagueData(string leagueId)
        {
            string leagueJSON = await Fetch.Get(Resources.LeagueData(leagueId));

            List<string> Ids = [];

            JObject leagueObject = JObject.Parse(leagueJSON);
            JArray allResultsObject = JArray.Parse(leagueObject["standings"]?["results"]?.ToString() ?? string.Empty);

            List<JToken> results = [.. allResultsObject.Children()];

            foreach (JToken result in results)
            {
                string team = result["entry"]?.ToString() ?? string.Empty;
                Ids.Add(team);
            }

            if (Ids.Count == 0)
            {
                return "No teams found in league";
            }

            if (Ids.Count > 20)
            {
                Ids = Ids.GetRange(0, 20);
            }

            List<Task<string>> tasks = [];

            foreach (string teamId in Ids)
            {
                tasks.Add(Fetch.Get(Resources.TeamHistory(teamId)));
            }

            string[] completedTasks = await Task.WhenAll(tasks);

            string res = JsonSerializer.Serialize(completedTasks);

            return res;
        }

        public class LeagueRequest
        {
            public required string LeagueId { get; set; }
        }


        [HttpPost(Name = "PostLeagueHistory")]
        public async Task<string> Post([FromBody] LeagueRequest request)
        {
            return await CompileLeagueData(request.LeagueId);
        }
    }
}
