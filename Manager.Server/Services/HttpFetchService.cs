using Manager.Server.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Manager.Server.Services
{
    public class HttpFetchService: IHttpFetchService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(5);

        public HttpFetchService(IMemoryCache cache, IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string?> Get(string url, TimeSpan? cacheDuration = null)
        {
            var duration = cacheDuration ?? DefaultCacheDuration;

            if (_cache.TryGetValue(url, out string? cachedResponse) && !string.IsNullOrEmpty(cachedResponse))
            {
                return cachedResponse;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var data = await response.Content.ReadAsStringAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(duration);

            _cache.Set(url, data, cacheEntryOptions);
            return data;
        }
    }
}