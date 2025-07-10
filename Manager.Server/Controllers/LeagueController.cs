using Manager.Server.Interfaces;
using Manager.Server.Models;
using Manager.Server.Services;
using Manager.Server.Shared;
using Manager.Server.Source;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeagueController : ControllerBase
    {

        private readonly ILogger<LeagueController> _logger;
        private readonly IHttpFetchService _fetchService;
        private readonly ILiveDataService _liveDataService;
        private readonly Cache _cache;

        public LeagueController(ILogger<LeagueController> logger, IHttpFetchService fetchService, ILiveDataService liveDataService, Cache cache)
        {
            _logger = logger;
            _fetchService = fetchService;
            _liveDataService = liveDataService;
            _cache = cache;
        }

        private async Task<string> GetLeagueData(int leagueId)
        {
            League league = new()
            {
                Id = leagueId,
                Week = _cache.Week
            };

            string JSON = await _fetchService.Get(Resources.LeagueData(leagueId));
            JObject leagueObject = JObject.Parse(JSON);

            league.Name = leagueObject["league"]?["name"]?.ToString() ?? string.Empty;

            JArray results = JArray.Parse(leagueObject["standings"]?["results"]?.ToString() ?? string.Empty);

            List<JToken> managers = [.. results.Children()];

            league.Size = managers.Count;

            foreach (JToken result in managers)
            {
                TeamManager manager = new()
                {
                    Id = int.Parse(result["entry"]?.ToString() ?? string.Empty),
                    Name = result["entry_name"]?.ToString() ?? string.Empty,
                    ManagerName = result["player_name"]?.ToString() ?? string.Empty,
                    TotalPoints = int.Parse(result["total"]?.ToString() ?? "0"),
                    GameweekPoints = int.Parse(result["event_total"]?.ToString() ?? "0"),
                    Rank = int.Parse(result["rank"]?.ToString() ?? "0"),
                    LastRank = int.Parse(result["last_rank"]?.ToString() ?? "0")
                };
                league.Teams.Add(manager);
            }

            string res = JsonSerializer.Serialize(league, JsonOptionsProvider.Options);
            return res;
        }


        /// <summary>
        /// Gets the player ids for the top 30 teams in a league
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        private async Task<List<Team>> GetLeagueIds(int leagueId)
        {
            string leagueJSON = await _fetchService.Get(Resources.LeagueData(leagueId));

            List<Team> Ids = [];

            JObject leagueObject = JObject.Parse(leagueJSON);
            JArray allResultsObject = JArray.Parse(leagueObject["standings"]?["results"]?.ToString() ?? string.Empty);

            List<JToken> results = [.. allResultsObject.Children()];

            foreach (JToken result in results)
            {
                int teamId = int.Parse(result["entry"]?.ToString() ?? string.Empty);
                string teamName = result["player_name"]?.ToString() ?? string.Empty;
                string managerName = result["entry_name"]?.ToString() ?? string.Empty;
                Ids.Add(new Team(teamId, teamName, managerName));
            }

            if (Ids.Count == 0)
            {
                return Ids;
            }

            if (Ids.Count > 30)
            {
                Ids = Ids.GetRange(0, 30);
            }
            return Ids;
        }

        private async Task<string> GetLeagueHistory(int leagueId)
        {
            List<Team> Ids = await GetLeagueIds(leagueId);

            List<Task<TeamHistory>> tasks = [];

            foreach (Team team in Ids)
            {
                tasks.Add(GetTeamHistory(team));
            }

            TeamHistory[] completedTasks = await Task.WhenAll(tasks);

            string res = JsonSerializer.Serialize(completedTasks, JsonOptionsProvider.Options);
            return res;
        }

        private async Task<TeamHistory> GetTeamHistory(Team team)
        {
            TeamHistory teamHistory = new(team.Id, team.Name);

            string teamHistoryJSON = await _fetchService.Get(Resources.TeamHistory(team.Id));

            JObject teamHistoryObject = JObject.Parse(teamHistoryJSON);
            JArray currentHistory = JArray.FromObject(teamHistoryObject["current"] ?? string.Empty);

            foreach (JObject history in currentHistory.Cast<JObject>())
            {
                teamHistory.Points.Add(int.Parse(history["points"]?.ToString() ?? string.Empty));
                teamHistory.Total_points.Add(int.Parse(history["total_points"]?.ToString() ?? string.Empty));
                teamHistory.Points_on_bench.Add(int.Parse(history["points_on_bench"]?.ToString() ?? string.Empty));
            }
            return teamHistory;
        }


        private async Task<string> GetManagerPicks(int leagueId)
        {
            List<Team> Ids = await GetLeagueIds(leagueId);

            List<Task<ManagerPicks>> tasks = [];

            foreach (Team team in Ids)
            {
                tasks.Add(_liveDataService.GetPicks(team));
            }

            ManagerPicks[] completedTasks = await Task.WhenAll(tasks);

            string res = JsonSerializer.Serialize(completedTasks, JsonOptionsProvider.Options);
            return res;
        }

        [HttpGet("{leagueId}")]
        public async Task<string> Get([FromRoute] int leagueId)
        {
            return await GetLeagueData(leagueId);
        }

        [HttpGet("{leagueId}/history")]
        public async Task<string> GetHistory([FromRoute] int leagueId)
        {
            return await GetLeagueHistory(leagueId);
        }

        [HttpGet("{leagueId}/picks")]
        public async Task<string> GetPicks([FromRoute] int leagueId)
        {
            return await GetManagerPicks(leagueId);
        }
    }
}
