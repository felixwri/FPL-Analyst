namespace Manager.Server.Shared
{
    public class Resources
    {
        private const string BaseUrl = "https://fantasy.premierleague.com/api";

        public static string LeagueData(int leagueId) =>
            $"{BaseUrl}/leagues-classic/{leagueId}/standings/";

        public static string TeamHistory(int teamId) =>
            $"{BaseUrl}/entry/{teamId}/history/";

        public static string TeamData(int teamId) =>
            $"{BaseUrl}/entry/{teamId}/";

        public static string ManagerPicks(int teamId, int gameweek) =>
            $"{BaseUrl}/entry/{teamId}/event/{gameweek}/picks/";

        public static string LivePlayerData(int gameweek) =>
            $"{BaseUrl}/event/{gameweek}/live/";

        public static string Fixtures() =>
            $"{BaseUrl}/fixtures/";

        public static string Bootstrap() =>
            $"{BaseUrl}/bootstrap-static/";
    }
}