using Manager.Server.Interfaces;
using Manager.Server.Models;
using Manager.Server.Shared;
using Manager.Server.Source;
using Newtonsoft.Json.Linq;

namespace Manager.Server.Services
{
    public class LiveDataService: ILiveDataService
    {
        private readonly IHttpFetchService _fetchService;
        private readonly Cache _cache;
        public LiveDataService(IHttpFetchService fetchService, Cache cache)
        {
            _fetchService = fetchService;
            _cache = cache;
        }

        /// <summary>
        /// Fetches the live player data for a given team and returns a dictionary with
        /// the player id as the key
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task<Dictionary<int, PlayerData>> GetLivePlayerData()
        {
            string liveDataJSON = await _fetchService.Get(Resources.LivePlayerData(_cache.Week));

            JObject livePlayerData = JObject.Parse(liveDataJSON);
            JArray results = JArray.Parse(livePlayerData["elements"]?.ToString() ?? string.Empty);

            List<JToken> players = [.. results.Children()];

            Dictionary<int, PlayerData> livePlayerDataAssignment = [];

            Dictionary<int, PlayerData> playerAssignment = _cache.Players;

            foreach (JToken result in players)
            {
                int playerId = int.Parse(result["id"]?.ToString() ?? string.Empty);

                int livePoints = result["stats"]?["total_points"]?.ToObject<int>() ?? 0;


                PlayerData playerData = new()
                {
                    IsLive = true,
                    Id = playerId,

                    FirstName = playerAssignment[playerId].FirstName,
                    SecondName = playerAssignment[playerId].SecondName,

                    TeamId = playerAssignment[playerId].TeamId,
                    TeamName = playerAssignment[playerId].TeamName,

                    Position = playerAssignment[playerId].Position,
                    PositionShort = playerAssignment[playerId].PositionShort,
                    ImageCode = playerAssignment[playerId].ImageCode,

                    WeekPoints = livePoints,
                    TotalPoints = playerAssignment[playerId].TotalPoints + livePoints,
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

        public async Task<ManagerPicks> GetPicks(Team team)
        {
            ManagerPicks managerPicks = new()
            {
                Id = team.Id,
                ManagerName = team.ManagerName,
                Name = team.Name,
                IsLive = _cache.GameWeek.IsActive,
                Gameweek = _cache.Week,
            };

            Dictionary<int, PlayerData> playerAssignment;

            if (_cache.GameWeek.IsActive)
            {
                playerAssignment = await GetLivePlayerData();
            }
            else
            {
                playerAssignment = _cache.Players;
            }

            string managerPicksJSON = await _fetchService.Get(Resources.ManagerPicks(team.Id, _cache.Week));

            JObject managerPicksObject = JObject.Parse(managerPicksJSON);
            JArray managerPicksArray = JArray.FromObject(managerPicksObject["picks"] ?? string.Empty);

            foreach (JObject pick in managerPicksArray.Cast<JObject>())
            {
                int playerId = int.Parse(pick["element"]?.ToString() ?? string.Empty);
                PlayerData playerData = playerAssignment[playerId];
                playerData.Multiplier = int.Parse(pick["multiplier"]?.ToString() ?? "1");
                playerData.BenchOrder = int.Parse(pick["position"]?.ToString() ?? "0");
                managerPicks.Picks.Add(playerData);
            }
            return managerPicks;
        }
    }
}