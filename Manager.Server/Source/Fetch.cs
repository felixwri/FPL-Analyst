using Microsoft.Extensions.Caching.Memory;

namespace Manager.Server.Source
{

    public class Fetch
    {
        private static readonly MemoryCache Cache = new(new MemoryCacheOptions { SizeLimit = 10_000 });
        private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(5);

        public static async Task<string> Get(string url, TimeSpan CacheDuration = default)
        {
            if (CacheDuration == default)
            {
                CacheDuration = DefaultCacheDuration;
            }

            if (Cache.TryGetValue(url, out string? cachedResponse))
            {
                if (cachedResponse != null && cachedResponse != "None")
                {
                    return cachedResponse;
                }
            }

            using HttpClient client = new();
            string apiUrl = url;


            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetAbsoluteExpiration(CacheDuration);

                Cache.Set(url, data, cacheEntryOptions);
                return data;
            }
            else
            {
                return "None";
            }
        }
    }
}