namespace CacheManager.Services
{
    public interface IApiEndPointsSetting
    {
        string BookEndPoint { get; set; }
        string CommentEndPoint { get; set; }

        string GetBookUrl(string id);
        string GetCommentUrl(string id);
    }

}
