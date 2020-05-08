using System;
using System.Threading.Tasks;

namespace CacheManager.Services
{
    public interface ICommentService
    {
        Task<Comment> GetComment(int id);
    }
}
