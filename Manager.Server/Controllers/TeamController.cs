using Manager.Server.Source;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Server.Controllers
{
    public class FPLRequest
    {
        public required string TeamId { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private static async Task<string> GetTeamData(string teamID)
        {
            return await Fetch.Get(Resources.TeamData(teamID));
        }

        [HttpGet("{teamId}")]
        public async Task<string> Post([FromRoute] string teamId)
        {
            return await GetTeamData(teamId);
        }
    }
}
