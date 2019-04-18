using System;
using System.Threading.Tasks;

namespace Dapper.Net.Caching {

    public interface ICacheProvider {
        object Get(string key);
        T GetWithRefresh<T>(string key, Func<T> refreshFunc, int cacheTime = 360) where T : class;
        Task<T> GetWithRefreshAsync<T>(string key, Func<Task<T>> refreshFuncAsync, int cacheTime = 360) where T : class;
        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        void Invalidate(string key);
    }

}
