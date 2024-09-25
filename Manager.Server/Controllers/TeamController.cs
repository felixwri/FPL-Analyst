using System.Text.Json;
using Manager.Server.Source;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private static async Task<string> GetTeamData(int teamID)
        {
            return await Fetch.Get(Resources.TeamData(teamID));
        }

        [HttpGet("{teamId}")]
        public async Task<string> Get([FromRoute] int teamId)
        {
            return await GetTeamData(teamId);
        }

        [HttpGet("{teamId}/players")]
        public async Task<string> GetPlayers([FromRoute] int teamId)
        {
            Team team = new()
            {
                Id = teamId,
            };

            ManagerPicks picks = await Processing.GetPicks(team);

            return JsonSerializer.Serialize(picks, JsonOptionsProvider.Options);
        }
    }
}
