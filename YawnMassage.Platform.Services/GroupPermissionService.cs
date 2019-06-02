using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class GroupPermissionService : IGroupPermissionService
    {
        private readonly ISystemDataContext _systemDataContext;
        private readonly IGroupDataContextFactory _groupDataContextFactory;

        public GroupPermissionService(ISystemDataContext systemDataContexty, IGroupDataContextFactory groupDataContextFactory)
        {
            _systemDataContext = systemDataContexty;
            _groupDataContextFactory = groupDataContextFactory;
        }

        public virtual async Task<List<PermissionConfigDto>> GetGroupRolesForSpecificDomainAsync(string domainPrefix, string groupId)
        {
            var _groupDataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

            var permissions = (await _groupDataContext.GetDocumentsAsync<PermissionConfig, PermissionConfig>(q => q)).ToList()
                               .Where(p => p.Permissions.Where(ps => ps.StartsWith(domainPrefix)).Any()).
                              Select(pc => new PermissionConfigDto()
                              {
                                  GroupId = pc.GroupId,
                                  Role = pc.Role,
                                  Permissions = pc.Permissions.Where(s => s.StartsWith(domainPrefix)).Select(c => c).ToList()
                              }).ToList();

            return permissions;
        }

    }
}
