using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace YawnMassage.Api.Helpers
{
    public class InMemoryTicketStore : ITicketStore
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryTicketStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            _memoryCache.Set(key, ticket);
            return Task.CompletedTask;
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var ticket = _memoryCache.Get<AuthenticationTicket>(key);
            return Task.FromResult(ticket);
        }

        public Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = Guid.NewGuid().ToString().ToLower();
            _memoryCache.Set(key, ticket);
            return Task.FromResult(key);
        }
    }
}
