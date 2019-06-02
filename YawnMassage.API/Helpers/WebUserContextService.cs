using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Api.Helpers
{
    public class WebUserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RequestContext _requestContext;

        public const string RefreshClientSessionHeader = "X-RefreshClientSession";

        public WebUserContextService(IHttpContextAccessor httpContextAccessor, RequestContext requestContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _requestContext = requestContext;
        }

        public async Task<IEnumerable<string>> GetClaimsForCurrentUserAsync()
        {
            var permissions = _httpContextAccessor.HttpContext.User.Claims.Select(c=>c.Value);
            return await Task.FromResult(permissions);
        }

        public async Task<bool> HasPermissionAsync(string permission)
        {
            var claim = $"{_requestContext.GroupId}:{permission}";
            bool hasClaim = _httpContextAccessor.HttpContext.User.HasClaim(CustomClaims.Permission, claim);
            return await Task.FromResult(hasClaim);
        }

        /// <summary>
        /// Sets a resposne header which informs the client to refresh any cached user session data.
        /// </summary>
        public void RefreshClientSessionData()
        {
            _httpContextAccessor.HttpContext.Response.Headers[RefreshClientSessionHeader] = "true";
        }
    }
}
;