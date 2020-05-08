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

    public class BookService : IBookService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<int, Book> _bookRepository;

        public BookService(ICacheManager cacheManager, IRepository<int, Book> bookRepository)
        {
            _cacheManager = cacheManager;
           
            _bookRepository = bookRepository;
        }
        public async Task<Book> GetBook(int id)
        {
            return await _cacheManager.GetAsync<int, Book>(id, async (bookId) => await _bookRepository.GetAsync(bookId));
            //return await _cacheManager.GetAsync<int, Book>(id, async (bookId) => await Task.FromResult(new Book() { Id = bookId,  }));
        }


        
    }
}
