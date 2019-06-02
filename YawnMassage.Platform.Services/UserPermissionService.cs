using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IGroupDataContextFactory _groupDataContextFactory;
        private readonly ISystemDataContext _systemDataContext;

        public UserPermissionService(IGroupDataContextFactory groupDataContextFactory, ISystemDataContext systemDataContext)
        {
            _groupDataContextFactory = groupDataContextFactory;
            _systemDataContext = systemDataContext;
        }

        public async Task<List<string>> GetEffectivePermissionsForUserAsync(User user, string contextGroupId)
        {
            return await GetEffectivePermissionsForUserByGroup(user, contextGroupId);
        }

        public async Task<List<string>> GetEffectivePermissionsForUserAsync(User user)
        {
            return await GetEffectivePermissionsForUserByGroup(user);
        }

        private async Task<List<string>> GetEffectivePermissionsForUserByGroup(User user, string contextGroupId = null)
        {
            var allPermissions = new List<string>();

            List<UserGroupRole> userGroupRoleList = user.GroupRoles;
            //Check if "ANY" to prevent group filteration
            if (contextGroupId != null && contextGroupId != "ANY")
            {
                userGroupRoleList = userGroupRoleList.Where(c => c.Group == contextGroupId).ToList();
            }

            var roleGroups = userGroupRoleList.GroupBy(cr => cr.Group, cr => cr.Role);
            foreach (var roleGroup in roleGroups)
            {
                string groupId = roleGroup.Key;
                var roles = roleGroup.ToList();

                //The ALL_ROLES role. Whatever permissions defined at this role should merge
                //with permissions defined at the specific named role.
                roles.Add("*");

                //Check whether the group has their own LIST_ROLES defined.
                var groupDataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);
                var groupListRolesDefined = await groupDataContext.AnyAsync<Lookup>(q => q.Where(l => l.Key == SystemKeys.LookupKeys.LIST_ROLES));

                foreach (string role in roles)
                {
                    var config = await groupDataContext.FirstOrDefaultAsync<PermissionConfig, PermissionConfig>(q =>
                                q.Where(pc => pc.Role == role));

                    //Fallback to system-level permissions only if group does not have their own LIST_ROLES defined.
                    if (config == null && !groupListRolesDefined)
                    {
                        config = await _systemDataContext.FirstOrDefaultAsync<PermissionConfig, PermissionConfig>(q =>
                                q.Where(pc => pc.Role == role));
                    }

                    if (config != null)
                        allPermissions.AddRange(config.Permissions.Select(p => $"{groupId}:{p}"));
                }
            }

            return allPermissions.OrderBy(s => s).Distinct().ToList();
        }
    }
}
