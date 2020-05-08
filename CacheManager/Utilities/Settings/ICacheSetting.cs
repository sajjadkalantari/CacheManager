namespace CacheManager.Services
{
    public interface ICacheSetting
    {
        int Level { get; set; }
        int AbsoluteExpirationDay { get; set; }
        int AbsoluteExpirationRelativeToNowInMinutes { get; set; }
    }

}
