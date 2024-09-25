using Newtonsoft.Json.Linq;

namespace Manager.Server.Source
{
    public class Processing
    {
        /// <summary>
        /// Fetches the live player data for a given team and returns a dictionary with
        /// the player id as the key
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public static async Task<Dictionary<int, PlayerData>> GetLivePlayerData()
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

        public static async Task<ManagerPicks> GetPicks(Team team)
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
                playerAssignment = await Processing.GetLivePlayerData();
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
    }
}