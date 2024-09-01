using Microsoft.AspNetCore.Mvc;

namespace Manager.Server.Controllers
{
    public class FPLRequest
    {
        public required string TeamId { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {

        private static async Task<string> GetGlobalData()
        {
            using HttpClient client = new();
            string apiUrl = "https://fantasy.premierleague.com/api/fixtures/";

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


        private static async Task<string> GetTeamData(string teamID)
        {
            using HttpClient client = new();
            string apiUrl = $"https://fantasy.premierleague.com/api/entry/{teamID}/";

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

        [HttpGet(Name = "GetTeam")]
        public async Task<string> Get()
        {
            return await GetGlobalData();
        }

        [HttpPost(Name = "PostTeam")]
        public async Task<string> Post([FromBody] FPLRequest request)
        {
            return await GetTeamData(request.TeamId);
        }
    }
}
