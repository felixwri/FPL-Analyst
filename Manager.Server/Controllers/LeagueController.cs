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

        /// <summary>
        /// Gets the player ids for the top 30 teams in a league
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        private static async Task<List<TeamHistory>> GetLeagueIds(string leagueId)
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
                return Ids;
            }

            if (Ids.Count > 30)
            {
                Ids = Ids.GetRange(0, 30);
            }
            return Ids;
        }

        private static async Task<string> GetLeagueHistory(string leagueId)
        {
            List<TeamHistory> Ids = await GetLeagueIds(leagueId);

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


        private static async Task<string> GetManagerPicks(string leagueId)
        {
            List<TeamHistory> Ids = await GetLeagueIds(leagueId);

            List<Task<ManagerPicks>> tasks = [];

            foreach (TeamHistory team in Ids)
            {
                tasks.Add(GetPicks(team));
            }

            ManagerPicks[] completedTasks = await Task.WhenAll(tasks);

            string res = JsonSerializer.Serialize(completedTasks, JsonOptionsProvider.Options);
            return res;
        }

        private static async Task<ManagerPicks> GetPicks(TeamHistory team)
        {
            ManagerPicks managerPicks = new()
            {
                Id = team.Id,
                Name = team.Name,
                IsLive = Cache.Instance.GameWeek.IsActive,
                Gameweek = Cache.Instance.Week,
            };

            Dictionary<int, PlayerData> playerAssignment;

            if (Cache.Instance.GameWeek.IsActive)
            {
                playerAssignment = await GetLivePlayerData(team);
            }
            else
            {
                playerAssignment = Cache.Instance.Players;
            }

            string managerPicksJSON = await Fetch.Get(Resources.ManagerPicks(team.Id, Cache.Instance.Week));

            JObject managerPicksObject = JObject.Parse(managerPicksJSON);
            JArray managerPicksArray = JArray.FromObject(managerPicksObject["picks"] ?? string.Empty);

            foreach (JObject pick in managerPicksArray.Cast<JObject>())
            {
                int playerId = int.Parse(pick["element"]?.ToString() ?? string.Empty);
                PlayerData playerData = playerAssignment[playerId];
                playerData.Multiplier = int.Parse(pick["multiplier"]?.ToString() ?? "1");
                managerPicks.Picks.Add(playerData);
            }
            return managerPicks;
        }


        /// <summary>
        /// Fetches the live player data for a given team and returns a dictionary with
        /// the player id as the key
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private static async Task<Dictionary<int, PlayerData>> GetLivePlayerData(TeamHistory team)
        {
            string liveDataJSON = await Fetch.Get(Resources.LivePlayerData(Cache.Instance.Week));

            JObject livePlayerData = JObject.Parse(liveDataJSON);
            JArray results = JArray.Parse(livePlayerData["elements"]?.ToString() ?? string.Empty);

            List<JToken> players = [.. results.Children()];

            Dictionary<int, PlayerData> livePlayerDataAssignment = [];

            Dictionary<int, PlayerData> playerAssignment = Cache.Instance.Players;

            foreach (JToken result in players)
            {
                int playerId = int.Parse(result["id"]?.ToString() ?? string.Empty);

                PlayerData playerData = new()
                {
                    IsLive = true,
                    Id = playerId,

                    FirstName = playerAssignment[playerId].FirstName,
                    SecondName = playerAssignment[playerId].SecondName,

                    TeamId = playerAssignment[playerId].TeamId,
                    TeamName = playerAssignment[playerId].TeamName,

                    TotalPoints = result["stats"]?["total_points"]?.ToObject<int>() ?? 0,
                    BonusPoints = result["stats"]?["bonus"]?.ToObject<int>() ?? 0,
                    Minutes = result["stats"]?["minutes"]?.ToObject<int>() ?? 0,
                    GoalsScored = result["stats"]?["goals_scored"]?.ToObject<int>() ?? 0,
                    ExpectedGoalsScored = result["stats"]?["expected_goals"]?.ToString() ?? string.Empty,
                    Assists = result["stats"]?["assists"]?.ToObject<int>() ?? 0,
                    CleanSheets = result["stats"]?["clean_sheets"]?.ToObject<int>() ?? 0,
                    GoalsConceded = result["stats"]?["goals_conceded"]?.ToObject<int>() ?? 0,
                    OwnGoals = result["stats"]?["own_goals"]?.ToObject<int>() ?? 0,
                    Saves = result["stats"]?["saves"]?.ToObject<int>() ?? 0

                };

                livePlayerDataAssignment.Add(playerData.Id, playerData);
            }
            return livePlayerDataAssignment;
        }

        [HttpGet("{leagueId}")]
        public async Task<string> Get([FromRoute] string leagueId)
        {
            return await GetLeagueData(leagueId);
        }

        [HttpGet("{leagueId}/history")]
        public async Task<string> GetHistory([FromRoute] string leagueId)
        {
            return await GetLeagueHistory(leagueId);
        }

        [HttpGet("{leagueId}/picks")]
        public async Task<string> GetPicks([FromRoute] string leagueId)
        {
            return await GetManagerPicks(leagueId);
        }
    }
}
