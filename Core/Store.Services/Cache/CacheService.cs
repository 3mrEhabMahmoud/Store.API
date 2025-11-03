using Store.Domain.Contracts;
using Store.Services.Abstractions.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Cache
{
    public class CacheService(ICacheRepository _cacheRepository) : ICacheService
    {
        public async Task<string?> GetAsync(string Key)
        {
            var resuit = await _cacheRepository.GetAsync(Key);
            return resuit;
        }

        public async Task SetAsync(string Key, object value, TimeSpan duration)
        {
            await _cacheRepository.SetAsync(Key, value, duration);
        }
    }
}
