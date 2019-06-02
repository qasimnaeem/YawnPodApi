using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Documents;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IUserPermissionService" />
    /// </summary>
    public interface IUserPermissionService
    {
        /// <summary>
        /// The GetEffectivePermissionsForUserAsync
        /// </summary>
        /// <param name="user">The user<see cref="User"/></param>
        /// <returns>The <see cref="Task{List{string}}"/></returns>
        Task<List<string>> GetEffectivePermissionsForUserAsync(User user);

        /// <summary>
        /// The GetEffectivePermissionsForUserAsync
        /// </summary>
        /// <param name="user">The user<see cref="User"/></param>
        /// <param name="contextGroupId">The contextGroupId<see cref="string"/></param>
        /// <returns>The <see cref="Task{List{string}}"/></returns>
        Task<List<string>> GetEffectivePermissionsForUserAsync(User user, string contextGroupId);
    }
}
