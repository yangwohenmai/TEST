using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using static System.DateTime;
using static System.TimeSpan;

namespace Dapper.Net.Caching {

    public class DefaultCacheProvider : ICacheProvider {
        public static Lazy<DefaultCacheProvider> LazyCache = new Lazy<DefaultCacheProvider>(() => new DefaultCacheProvider()); 
        private static ObjectCache Cache => MemoryCache.Default;
        public object Get(string key) => Cache[key];

        public T GetWithRefresh<T>(string key, Func<T> refreshFunc, int cacheTime = 360) where T : class {
            var cached = Get(key) as T;
            return cached ?? Set(key, refreshFunc(), cacheTime);
        }

        public async Task<T> GetWithRefreshAsync<T>(string key, Func<Task<T>> refreshFuncAsync, int cacheTime = 360) where T : class {
            var cached = Get(key) as T;
            return cached ?? Set(key, await refreshFuncAsync(), cacheTime);
        }

        public T Set<T>(string key, T data, int cacheTime) where T : class {
            if (data == null) return null;
            var policy = new CacheItemPolicy {AbsoluteExpiration = Now + FromMinutes(cacheTime)};
            Cache.Add(new CacheItem(key, data), policy);
            return data;
        }

        public void Set(string key, object data, int cacheTime) => Set<object>(key, data, cacheTime);
        public bool IsSet(string key) => (Cache[key] != null);
        public void Invalidate(string key) => Cache.Remove(key);
    }

}
