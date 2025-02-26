using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System.Text.Json;

namespace Zen.Infrastructure.Cache
{
    /// <summary>
    /// A Redis cache implementation of ICacheService.
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        private readonly StackExchange.Redis.IDatabase _database;

        public RedisCacheService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            var redis = ConnectionMultiplexer.Connect(connectionString);
            _database = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, expiry);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var json = await _database.StringGetAsync(key);
            if (string.IsNullOrEmpty(json))
                return default;
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}