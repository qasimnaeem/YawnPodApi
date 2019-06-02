using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Platform.Domain.Dto.Group;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IGroupService" />
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="gridCriteria">The gridCriteria<see cref="ResultSetCriteria"/></param>
        /// <param name="searchCriteria">The searchCriteria<see cref="GroupSearchCriteria"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{GroupDto}}"/></returns>
        Task<PagedQueryResultSet<GroupDto>> GetAsync(ResultSetCriteria gridCriteria, GroupSearchCriteria searchCriteria);

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{GroupDto}"/></returns>
        Task<GroupDto> GetAsync(string id);

        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="groupDto">The groupDto<see cref="GroupDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateAsync(GroupDto groupDto);

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="groupDto">The groupDto<see cref="GroupDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> UpdateAsync(GroupDto groupDto);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="ids">The ids<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(params string[] ids);
    }
}
