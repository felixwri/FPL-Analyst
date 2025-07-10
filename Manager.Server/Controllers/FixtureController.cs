using System.Text.Json;
using Manager.Server.Shared;
using Manager.Server.Source;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FixtureController : ControllerBase
    {
        public readonly ILogger<FixtureController> _logger;
        public readonly Cache _cache;

        public FixtureController(ILogger<FixtureController> logger, Cache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        private string GetUpcomingFixtures()
        {
            string result = JsonSerializer.Serialize(_cache.AllUpcomingFixtures, JsonOptionsProvider.Options);
            return result;
        }


        [HttpGet(Name = "GetFixture")]
        public string Get()
        {
            return GetUpcomingFixtures();
        }
    }
}
