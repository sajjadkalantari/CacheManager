using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;

namespace CacheManager.Services
{
    public static class CacheManagerFactory
    {
        public static LinkedListNode<CacheWrapper> CreateCacheHierachy(List<IDistributedCache> distributedCaches, List<ICacheSetting> settings)
        {
            var cacheProviders = new LinkedList<CacheWrapper>();
           
            for (int i = 0; i < distributedCaches.Count; i++)            
                cacheProviders.AddLast(new CacheWrapper(distributedCaches[i], settings.FirstOrDefault(m => m.Level == i)));
              
            return cacheProviders.First;
        }
    }
}
