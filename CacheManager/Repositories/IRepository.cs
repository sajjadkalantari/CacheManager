using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheManager.Repositories
{
    public interface IRepository<TKey, TEntity>
    {
        Task<TEntity> GetAsync(TKey id);
    }

}
