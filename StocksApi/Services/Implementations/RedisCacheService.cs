using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using StocksApi.Services.Interfaces;

namespace StocksApi.Services.Implementations
{
    public class RedisCacheService /*: IRedisCacheService*/
    {
        private readonly IConnectionMultiplexer _redis;
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task<string?> GetAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, expiration);
        }
    }
}
