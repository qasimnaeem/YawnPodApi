using System.Collections.Generic;
using System.Threading.Tasks;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IUserContextService" />
    /// </summary>
    public interface IUserContextService
    {
        /// <summary>
        /// The GetClaimsForCurrentUserAsync
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{string}}"/></returns>
        Task<IEnumerable<string>> GetClaimsForCurrentUserAsync();

        /// <summary>
        /// The HasPermissionAsync
        /// </summary>
        /// <param name="claimSuffix">The claimSuffix<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> HasPermissionAsync(string claimSuffix);

        /// <summary>
        /// The RefreshClientSessionData
        /// </summary>
        void RefreshClientSessionData();
    }
}
