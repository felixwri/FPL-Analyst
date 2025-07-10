using Manager.Server.Interfaces;
using Manager.Server.Source;

namespace Manager.Server.Startup
{
    public class CacheInit
    {
        public static async Task InitialiseAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var fetchService = scope.ServiceProvider.GetRequiredService<IHttpFetchService>();
            var cache = services.GetRequiredService<Cache>();

            var prefetch = new BootstrapDataFetcher(fetchService);
            prefetch.SetCache(cache);

            cache.GameWeek = await prefetch.GetStaticContent(); ;
            cache.Positions = await prefetch.GetPositionAssignment(); ;
            cache.Teams = await prefetch.GetTeamAssignment(); ;
            cache.Fixtures = await prefetch.GetFixtures(); ;
            cache.Players = await prefetch.GetPlayerAssignment(); ;


            cache.Initialise();
        }
    }
}
