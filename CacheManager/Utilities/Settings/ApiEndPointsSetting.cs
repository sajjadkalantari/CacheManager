namespace CacheManager.Services
{
    public class ApiEndPointsSetting : IApiEndPointsSetting
    {
        public string BookEndPoint { get; set; }
        public string CommentEndPoint { get; set; }

        public string GetBookUrl(string id)
        {
            return BookEndPoint.Replace("{id}", id);
        }

        public string GetCommentUrl(string id)
        {
            return CommentEndPoint.Replace("{id}", id);
        }
    }

}
