using CAF.Core.Service;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Service.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        //private readonly Dictionary<string, object> inMemoryCache;
        private readonly IMemoryCache _memCache;
        public MemoryCacheService(IMemoryCache memCache)
        {
            //inMemoryCache = new Dictionary<string, object>();
            _memCache = memCache;
        }
        public void DeleteObject(string key)
        {
            _memCache.Remove(key);
            //if (inMemoryCache.ContainsKey(key))
            //    inMemoryCache.Remove(key);
        }

        public T GetObject<T>(string key)
        {
            return _memCache.Get<T>(key);
            //_memCache.TryGetValue(cacheKey, out catList)

            //if (!inMemoryCache.ContainsKey(key)) return default;

            //return (T)inMemoryCache[key];
        }

        public void SetObject<T>(string key, T data)
        {
            var cacheExpOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(10),
                Priority = CacheItemPriority.Normal
            };
            _memCache.Set(key, data, cacheExpOptions);
            //inMemoryCache[key] = data;
        }
    }
}
