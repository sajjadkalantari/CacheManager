namespace CacheManager.Services
{
    public class CacheSetting : ICacheSetting
    {
        public int Level { get; set; }
        public int AbsoluteExpirationDay { get; set; }
        public int AbsoluteExpirationRelativeToNowInMinutes { get; set; }
    }

}
