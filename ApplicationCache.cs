using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Authorization
{
    public class ApplicationCache<T> where T : class
    {
        private readonly IMemoryCache _cache;

        public ApplicationCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        protected virtual string KeySeparator { get; } = ":";

        protected virtual string GetKey(string key)
        {
            return nameof(T) + KeySeparator + key;
        }

        public Task<T> GetAsync(string key)
        {
            key = GetKey(key);
            var item = _cache.Get<T>(key);
            return Task.FromResult(item);
        }

        public Task SetAsync(string key, T item, TimeSpan expiration)
        {
            key = GetKey(key);
            _cache.Set(key, item, expiration);
            return Task.CompletedTask;
        }
        
        public Task RemoveAsync(string key)
        {
            key = GetKey(key);
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}