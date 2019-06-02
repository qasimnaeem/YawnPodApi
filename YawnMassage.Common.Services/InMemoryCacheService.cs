using YawnMassage.Common.Domain.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    public class InMemoryCacheService : ILocalCacheService
    {
        protected readonly IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public TValue GetValue<TValue>(string key)
        {
            return _memoryCache.Get<TValue>(key);
        }

        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
                _memoryCache.Remove(key);
        }

        public void SetValue<TValue>(string key, TValue value)
        {
            _memoryCache.Set(key, value);
        }

        public void SetValue<TValue>(string key, TValue value, TimeSpan expiration)
        {
            _memoryCache.Set(key, value, expiration);
        }

        public bool KeyExists(string key)
        {
            throw new NotImplementedException();
        }

        //This should be moved to default interface implementation when C# 8.0 is available.
        //https://github.com/dotnet/csharplang/issues/288
        public TValue GetOrSetValue<TValue>(string key, Func<TValue> resolver, TimeSpan? expiration = null)
        {
            var value = GetValue<TValue>(key);
            if (value == null)
            {
                value = resolver();
                if (expiration == null)
                    SetValue(key, value);
                else
                    SetValue(key, value, expiration.Value);
            }

            return value;
        }

        //This should be moved to default interface implementation when C# 8.0 is available.
        //https://github.com/dotnet/csharplang/issues/288
        public async Task<TValue> GetOrSetValueAsync<TValue>(string key, Func<Task<TValue>> asyncResolver, TimeSpan? expiration = null)
        {
            var value = GetValue<TValue>(key);
            if (value == null)
            {
                value = await asyncResolver();
                if (expiration == null)
                    SetValue(key, value);
                else
                    SetValue(key, value, expiration.Value);
            }

            return value;
        }
    }
}
