namespace Manager.Server.Source
{
    public class Resources
    {
        public static string LeagueData(string leagueId)
        {
            return $"https://fantasy.premierleague.com/api/leagues-classic/{leagueId}/standings/";
        }

        public static string TeamHistory(string teamID)
        {
            return $"https://fantasy.premierleague.com/api/entry/{teamID}/history/";
        }

        public static string TeamData(string teamID)
        {
            return $"https://fantasy.premierleague.com/api/entry/{teamID}/";
        }

        public static string ManagerPicks(string teamID, int gameweek)
        {
            return $"https://fantasy.premierleague.com/api/entry/{teamID}/event/{gameweek}/picks/";
        }

        public static string LivePlayerData(int gameweek)
        {
            return $"https://fantasy.premierleague.com/api/event/{gameweek}/live/";
        }

        public static string Fixtures()
        {
            return $"https://fantasy.premierleague.com/api/fixtures/";
        }

        public static string Bootstrap()
        {
            return $"https://fantasy.premierleague.com/api/bootstrap-static/";
        }
    }
}