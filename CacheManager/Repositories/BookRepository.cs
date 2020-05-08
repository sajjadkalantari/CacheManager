using CacheManager.Services;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CacheManager.Repositories
{
    public class BookRepository : IRepository<int, Book>
    {
        private readonly HttpClient _httpClient;
        private readonly IApiEndPointsSetting _apiEndPointsSetting;

        public BookRepository(HttpClient httpClient, IApiEndPointsSetting apiEndPointsSetting)
        {
            _httpClient = httpClient;
            _apiEndPointsSetting = apiEndPointsSetting;
        }
        public async Task<Book> GetAsync(int id)
        {
            var bookString = await _httpClient.GetStringAsync(_apiEndPointsSetting.GetBookUrl($"{id}"));

            if (string.IsNullOrEmpty(bookString))
                return null;

            return JsonSerializer.Deserialize<Book>(bookString);
        }

       
    }

}
