using StackExchange.Redis;
using Store.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsync(string Key)
        {
            var redisValue = await _database.StringGetAsync(Key);
            return redisValue;
        }

        public async Task SetAsync(string Key, object value, TimeSpan duration)
        {
            await _database.StringSetAsync(Key, JsonSerializer.Serialize(value), duration);
        }
    }
}
