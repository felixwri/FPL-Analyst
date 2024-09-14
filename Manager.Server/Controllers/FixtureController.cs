using System.Text.Json;
using Manager.Server.Source;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FixtureController : ControllerBase
    {
        readonly Cache cache = Cache.Instance;

        private string GetUpcomingFixtures()
        {
            string result = JsonSerializer.Serialize(cache.AllUpcomingFixtures, JsonOptionsProvider.Options);
            return result;
        }


        [HttpGet(Name = "GetFixture")]
        public string Get()
        {
            return GetUpcomingFixtures();
        }
    }
}
