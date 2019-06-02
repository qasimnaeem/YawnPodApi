using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Platform.Domain.Dto.Configuration;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IPermissionService" />
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{PermissionConfigDto}"/></returns>
        Task<PermissionConfigDto> GetAsync(string id);

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="gridCriteria">The gridCriteria<see cref="ResultSetCriteria"/></param>
        /// <param name="searchCriteria">The searchCriteria<see cref="PermissionSearchCriteria"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{PermissionConfigDto}}"/></returns>
        Task<PagedQueryResultSet<PermissionConfigDto>> GetAsync(ResultSetCriteria gridCriteria, PermissionSearchCriteria searchCriteria);

        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="dto">The dto<see cref="PermissionConfigDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateAsync(PermissionConfigDto dto);

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="dto">The dto<see cref="PermissionConfigDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> UpdateAsync(PermissionConfigDto dto);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="ids">The ids<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(params string[] ids);
    }
}
