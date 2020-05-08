using Microsoft.Extensions.Caching.Distributed;

namespace CacheManager.Services
{
    public class CacheWrapper
    {
        public CacheWrapper(IDistributedCache distributedCache, ICacheSetting cacheSetting)
        {
            DistributedCache = distributedCache;
            CacheSetting = cacheSetting;
        }

        public IDistributedCache DistributedCache { get; set; }
        public ICacheSetting CacheSetting { get; set; }
    }
}
