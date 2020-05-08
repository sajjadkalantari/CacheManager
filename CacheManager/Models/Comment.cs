using System;

namespace CacheManager.Services
{
    [Serializable]
    public class Comment
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string BookName { get; set; }
    }
}
