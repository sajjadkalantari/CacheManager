using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheManager.Services
{
    public class SampleCacheManager : ICacheManager
    {
        private readonly LinkedListNode<CacheWrapper> cacheLayers;

        public SampleCacheManager(LinkedListNode<CacheWrapper> cacheLayers)
        {
            this.cacheLayers = cacheLayers;
        }

        public async Task<TValue> GetAsync<TKey, TValue>(TKey key, Func<TKey, Task<TValue>> factory)
        {
            return await Get(key, cacheLayers, factory);
        }


        private async Task<TValue> Get<TKey, TValue>(TKey key, LinkedListNode<CacheWrapper> distributedCache, Func<TKey, Task<TValue>> factory)
        {
            var value = (await distributedCache.Value.DistributedCache.GetAsync(key.ToString())).Deserialize<TValue>();

            if (value != null)
                return value;

            value = distributedCache.Next != null ? await Get(key, distributedCache.Next, factory) : await factory.Invoke(key);

            await Set(key.ToString(), value.SerializeObj(), distributedCache.Value);

            return value;
        }

        private async Task Set(string key, byte[] value, CacheWrapper cache)
        {
            await cache.DistributedCache.SetAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cache.CacheSetting.AbsoluteExpirationRelativeToNowInMinutes),
                AbsoluteExpiration = new DateTimeOffset(DateTime.Today.AddDays(cache.CacheSetting.AbsoluteExpirationDay))
            });
        }
    }
}
