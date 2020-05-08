using System;
using System.Threading.Tasks;

namespace CacheManager.Services
{
    public interface ICacheManager
    {
        Task<TValue> GetAsync<TKey, TValue>(TKey key, Func<TKey, Task<TValue>> factory);
    }
}
