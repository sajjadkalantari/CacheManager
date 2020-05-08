using CacheManager.Services;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CacheManager.Repositories
{
    public class CommentRepository : IRepository<int, Comment>
    {
        private readonly HttpClient _httpClient;
        private readonly IApiEndPointsSetting _apiEndPointsSetting;

        public CommentRepository(HttpClient httpClient, IApiEndPointsSetting apiEndPointsSetting)
        {
            _httpClient = httpClient;
            _apiEndPointsSetting = apiEndPointsSetting;
        }
        public async Task<Comment> GetAsync(int id)
        {
            var bookString = await _httpClient.GetStringAsync(_apiEndPointsSetting.GetCommentUrl($"{id}"));

            if (string.IsNullOrEmpty(bookString))
                return null;

            return JsonSerializer.Deserialize<Comment>(bookString);
        }

       
    }

}
