namespace Manager.Server.Source
{
    public class Resources
    {
        public static string LeagueData(int leagueId)
        {
            return $"https://fantasy.premierleague.com/api/leagues-classic/{leagueId}/standings/";
        }

        public static string TeamHistory(int teamID)
        {
            return $"https://fantasy.premierleague.com/api/entry/{teamID}/history/";
        }

        public static string TeamData(int teamID)
        {
            return $"https://fantasy.premierleague.com/api/entry/{teamID}/";
        }

        public static string ManagerPicks(int teamID, int gameweek)
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