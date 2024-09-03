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
        private static async Task<string> GetTeamData(string teamID)
        {
            return await Fetch.Get(Resources.TeamData(teamID));
        }

        [HttpPost(Name = "PostTeam")]
        public async Task<string> Post([FromBody] FPLRequest request)
        {
            return await GetTeamData(request.TeamId);
        }
    }
}
