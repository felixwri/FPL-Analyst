using System.Text.Json;
using Manager.Server.Source;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeagueController : ControllerBase
    {
        private static async Task<string> GetLeagueData(string leagueId)
        {
            return await Fetch.Get(Resources.LeagueData(leagueId));
        }

        private static async Task<string> GetLeagueHistory(string leagueId)
        {
            string leagueJSON = await Fetch.Get(Resources.LeagueData(leagueId));

            List<TeamHistory> Ids = [];

            JObject leagueObject = JObject.Parse(leagueJSON);
            JArray allResultsObject = JArray.Parse(leagueObject["standings"]?["results"]?.ToString() ?? string.Empty);

            List<JToken> results = [.. allResultsObject.Children()];

            foreach (JToken result in results)
            {
                string teamId = result["entry"]?.ToString() ?? string.Empty;
                string teamName = result["player_name"]?.ToString() ?? string.Empty;
                Ids.Add(new TeamHistory(teamId, teamName));
            }

            if (Ids.Count == 0)
            {
                return "No teams found in league";
            }

            if (Ids.Count > 20)
            {
                Ids = Ids.GetRange(0, 20);
            }

            List<Task<TeamHistory>> tasks = [];

            foreach (TeamHistory team in Ids)
            {
                tasks.Add(GetTeamHistory(team));
            }

            TeamHistory[] completedTasks = await Task.WhenAll(tasks);

            string res = JsonSerializer.Serialize(completedTasks, JsonOptionsProvider.Options);
            return res;
        }

        private static async Task<TeamHistory> GetTeamHistory(TeamHistory team)
        {
            string teamHistoryJSON = await Fetch.Get(Resources.TeamHistory(team.Id));
            JObject teamHistoryObject = JObject.Parse(teamHistoryJSON);
            JArray currentHistory = JArray.FromObject(teamHistoryObject["current"] ?? string.Empty);

            foreach (JObject history in currentHistory.Cast<JObject>())
            {
                team.Points.Add(int.Parse(history["points"]?.ToString() ?? string.Empty));
                team.Total_points.Add(int.Parse(history["total_points"]?.ToString() ?? string.Empty));
                team.Points_on_bench.Add(int.Parse(history["points_on_bench"]?.ToString() ?? string.Empty));
            }
            return team;
        }


        [HttpGet("{leagueId}")]
        public async Task<string> Get([FromRoute] string leagueId)
        {
            return await GetLeagueData(leagueId);
        }

        [HttpGet("{LeagueId}/history")]
        public async Task<string> Post([FromRoute] string leagueId)
        {
            return await GetLeagueHistory(leagueId);
        }
    }
}
