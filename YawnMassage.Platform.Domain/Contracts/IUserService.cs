using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Identity;
using YawnMassage.Platform.Domain.Dto.User;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IUserService" />
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="gridCriteria">The gridCriteria<see cref="ResultSetCriteria"/></param>
        /// <param name="searchCriteria">The searchCriteria<see cref="UserSearchCriteria"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{UserDto}}"/></returns>
        Task<PagedQueryResultSet<UserDto>> GetAsync(ResultSetCriteria gridCriteria, UserSearchCriteria searchCriteria);

        /// <summary>
        /// The GetUserForEditAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{UserDto}"/></returns>
        Task<UserDto> GetUserForEditAsync(string id);

        /// <summary>
        /// The GetUserAsync
        /// </summary>
        /// <param name="userId">The userId<see cref="string"/></param>
        /// <returns>The <see cref="Task{User}"/></returns>
        Task<User> GetUserAsync(string userId);

        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="userDto">The userDto<see cref="UserDto"/></param>
        /// <param name="isBulkImport">The isBulkImport<see cref="bool"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateAsync(UserDto userDto, bool isBulkImport = false);

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="userDto">The userDto<see cref="UserDto"/></param>
        /// <param name="isBulkImport">The isBulkImport<see cref="bool"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> UpdateAsync(UserDto userDto, bool isBulkImport = false);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="ids">The ids<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(params string[] ids);

        /// <summary>
        /// The ActivateUserAsync
        /// </summary>
        /// <param name="userId">The userId<see cref="string"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> ActivateUserAsync(string userId);

        /// <summary>
        /// The GetRandomPINAsync
        /// </summary>
        /// <param name="groupIds">The groupIds<see cref="string"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        Task<int> GetRandomPINAsync(string groupIds);

        /// <summary>
        /// The GetEffectiveGroupForUserAsync
        /// </summary>
        /// <param name="userId">The userId<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        Task<string> GetEffectiveGroupForUserAsync(string userId);

        /// <summary>
        /// The GetUsersByGroupAsync
        /// </summary>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <returns>The <see cref="Task{List{ListItemDto}}"/></returns>
        Task<List<ListItemDto>> GetUsersByGroupAsync(string groupId);

        /// <summary>
        /// The GetUserIdsByGroupRoleAsync
        /// </summary>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <param name="role">The role<see cref="string"/></param>
        /// <returns>The <see cref="Task{List{string}}"/></returns>
        Task<List<string>> GetUserIdsByGroupRoleAsync(string groupId, string role);

        /// <summary>
        /// The AddGroupRoleToUserAsync
        /// </summary>
        /// <param name="userId">The userId<see cref="string"/></param>
        /// <param name="groupRole">The groupRole<see cref="UserGroupRole"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task AddGroupRoleToUserAsync(string userId, UserGroupRole groupRole);

        /// <summary>
        /// return true if user account already exist
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        Task<bool> CheckIfAccountExistAsync(string mobileNumber);
        /// <summary>
        /// return user 
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        Task<MobileAuthResultDto> GetPostAuthDataAsync(string mobileNumber);
    }
}
