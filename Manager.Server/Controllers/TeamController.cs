using System.Text.Json;
using Manager.Server.Models;
using Manager.Server.Source;
using Manager.Server.Shared;
using Manager.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Manager.Server.Services;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private readonly HttpFetchService _fetchService;
        private readonly ILiveDataService _liveDataService;
        private readonly Cache _cache;

        public TeamController(ILogger<TeamController> logger, HttpFetchService fetchService, ILiveDataService liveDataService, Cache cache)
        {
            _logger = logger;
            _fetchService = fetchService;
            _liveDataService = liveDataService;
            _cache = cache;
        }

        [HttpGet("{teamId}")]
        public async Task<string> Get([FromRoute] int teamId)
        {
            return await _fetchService.Get(Resources.TeamData(teamId));
        }

        [HttpGet("{teamId}/players")]
        public async Task<string> GetPlayers([FromRoute] int teamId)
        {
            Team team = new()
            {
                Id = teamId,
            };

            ManagerPicks picks = await _liveDataService.GetPicks(team);

            return JsonSerializer.Serialize(picks, JsonOptionsProvider.Options);
        }
    }
}
