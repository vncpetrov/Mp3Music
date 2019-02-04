namespace Mp3MusicZone.Web.Infrastructure
{
    using Domain.Contracts;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;

    public class AspNetMemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache cache;

        public AspNetMemoryCacheManager(IMemoryCache cache)
        {
            if (cache is null)
                throw new ArgumentNullException(nameof(cache));

            this.cache = cache;
        }

        public void Add(string key, object item, int absoluteDurationInSeconds)
        {
            this.cache.Set(key, item, DateTime.UtcNow.AddSeconds(absoluteDurationInSeconds));
        }

        public bool Exists(string key)
        {
            return this.cache.Get(key) != null;
        }

        public T Get<T>(string key) where T : class
        {
            return this.cache.Get<T>(key);
        }

        public void Remove(string key)
        {
            this.cache.Remove(key);
        }
    }
}
