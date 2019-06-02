using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IUserPermissionService _userPermissionService;
        private readonly RequestContext _requestContext;
        private readonly ISystemDataContext _systemDataContext;

        public UserContextService(IUserPermissionService userPermissionService, RequestContext requestContext, ISystemDataContext systemDataContext)
        {
            _userPermissionService = userPermissionService;
            _requestContext = requestContext;
            _systemDataContext = systemDataContext;
        }

        public async Task<IEnumerable<string>> GetClaimsForCurrentUserAsync()
        {
            var user = await _systemDataContext.GetDocumentAsync<User>(_requestContext.UserId);
            var permissions = await _userPermissionService.GetEffectivePermissionsForUserAsync(user);
            return permissions;
        }

        public async Task<bool> HasPermissionAsync(string permission)
        {
            var claim = $"{_requestContext.GroupId}:{permission}";
            var userClaims = await GetClaimsForCurrentUserAsync();
            var hasClaim = userClaims.Contains(claim);
            return hasClaim;
        }

        /// <summary>
        /// Sets a resposne header which informs the client to refresh any cached user session data.
        /// </summary>
        public void RefreshClientSessionData()
        {
            
        }
    }
}
