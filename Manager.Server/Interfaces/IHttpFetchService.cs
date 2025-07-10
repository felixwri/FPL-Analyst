namespace Manager.Server.Interfaces
{
    public interface IHttpFetchService
    {
        Task<string?> Get(string url, TimeSpan? cacheDuration = null);
    }
}