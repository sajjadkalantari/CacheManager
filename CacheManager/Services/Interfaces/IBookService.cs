using System;
using System.Threading.Tasks;

namespace CacheManager.Services
{
    public interface IBookService
    {
        Task<Book> GetBook(int id);
    }
}
