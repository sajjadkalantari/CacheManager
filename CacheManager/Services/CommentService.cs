using CacheManager.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static CacheManager.Services.SampleCacheManager;

namespace CacheManager.Services
{

    public class CommentService : ICommentService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<int, Comment> _commentRepository;

        public CommentService(ICacheManager cacheManager, IRepository<int, Comment> bookRepository)
        {
            _cacheManager = cacheManager;
           
            _commentRepository = bookRepository;
        }        

        public async Task<Comment> GetComment(int id)
        {
            return await _cacheManager.GetAsync<int, Comment>(id, async (commentId) => await _commentRepository.GetAsync(commentId));
        }
    }
}
