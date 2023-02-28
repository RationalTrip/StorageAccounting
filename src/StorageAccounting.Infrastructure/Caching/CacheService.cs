using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StorageAccounting.Domain.Common;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(ILogger<CacheService> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task RemoveMultiple(CancellationToken token = default, params string[] keys)
        {
            var removeTasks = new Task[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                removeTasks[i] = _cache.RemoveAsync(keys[i], token);
            }

            await Task.WhenAll(removeTasks);
        }

        public async Task<Result<T>> GetOrSetFromSource<T>(Func<Task<Result<T>>> source,
            string key,
            CancellationToken token = default,
            [CallerMemberName] string calleName = "")
        {
            var cachedValue = await _cache.GetStringAsync(key, token);

            if (cachedValue is not null)
            {
                var cachedResult = JsonSerializer.Deserialize<T>(cachedValue);

                if (cachedResult is not null)
                    return cachedResult;

                LogBadCache(calleName, key, typeof(T).FullName ?? typeof(T).Name, cachedValue);
            }

            var result = await source();

            if (result.IsFaulted)
                return result;

            await _cache.SetStringAsync(key,
                JsonSerializer.Serialize<T>(result.Value),
                CacheOptions,
                token);

            return result;
        }

        private DistributedCacheEntryOptions CacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(15)
        };

        private void LogBadCache(string calleMethod, string cacheKey, string expectedTypeName, string cachedValue) =>
            _logger.LogWarning("Method {0} asked cached value with key {1} " +
                    "that can not be a type {2}. Readed value: '{3}'",
                    calleMethod,
                    cacheKey,
                    expectedTypeName,
                    cachedValue);
    }
}
