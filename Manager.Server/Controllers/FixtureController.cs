using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FixtureController : ControllerBase
    {
        readonly Cache cache = Cache.Instance;

        private string GetUpcomingFixtures()
        {
            string result = JsonSerializer.Serialize(cache.AllUpcomingFixtures);
            return result;
        }


        [HttpGet(Name = "GetFixture")]
        public string Get()
        {
            return GetUpcomingFixtures();
        }
    }
}
