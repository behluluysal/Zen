namespace Zen.Infrastructure.Cache
{
    /// <summary>
    /// Defines basic caching operations.
    /// </summary>
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
    }
}