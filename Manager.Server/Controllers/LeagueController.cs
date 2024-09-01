using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeagueController : ControllerBase
    {
        private static async Task<string> GetLeagueData(string leagueId)
        {
            using HttpClient client = new();
            string apiUrl = $"https://fantasy.premierleague.com/api/leagues-classic/{leagueId}/standings/";

            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }
            else
            {
                // Handle error response
                return "None";
            }
        }


        [HttpGet("{leagueId}")]
        public async Task<string> Get([FromRoute] string leagueId)
        {
            return await GetLeagueData(leagueId);
        }
    }
}
