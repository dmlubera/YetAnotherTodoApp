using Microsoft.Extensions.Caching.Memory;
using System;
using YetAnotherTodoApp.Application.Cache;

namespace YetAnotherTodoApp.Infrastructure.Cache
{
    public class MemoryCache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
            => _memoryCache.Get<T>(key);

        public void Set<T>(string key, T value, TimeSpan expirationTime)
            => _memoryCache.Set(key, value, expirationTime);
    }
}
