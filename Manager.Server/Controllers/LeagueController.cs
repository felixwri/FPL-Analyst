using Microsoft.AspNetCore.Mvc;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeagueController : ControllerBase
    {
        private static async Task<string> GetLeagueData(string leagueId)
        {
            return await Fetch.Get(Resources.LeagueData(leagueId));
        }


        [HttpGet("{leagueId}")]
        public async Task<string> Get([FromRoute] string leagueId)
        {
            return await GetLeagueData(leagueId);
        }
    }
}
