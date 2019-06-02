using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Dto.Configuration;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IGroupPermissionService" />
    /// </summary>
    public interface IGroupPermissionService
    {
        /// <summary>
        /// The GetGroupRolesForSpecificDomainAsync
        /// </summary>
        /// <param name="domainPrefix">The domainPrefix<see cref="string"/></param>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <returns>The <see cref="Task{List{PermissionConfigDto}}"/></returns>
        Task<List<PermissionConfigDto>> GetGroupRolesForSpecificDomainAsync(string domainPrefix, string groupId);
    }
}
