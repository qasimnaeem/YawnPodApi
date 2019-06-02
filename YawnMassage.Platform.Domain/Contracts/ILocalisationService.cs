using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Platform.Domain.Dto.Configuration;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ILocalisationService" />
    /// </summary>
    public interface ILocalisationService
    {
        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="localisationTextDto">The localisationTextDto<see cref="LocalisationTextDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateAsync(LocalisationTextDto localisationTextDto);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="ids">The ids<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(params string[] ids);

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{LocalisationTextDto}"/></returns>
        Task<LocalisationTextDto> GetAsync(string id);

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="gridCriteria">The gridCriteria<see cref="ResultSetCriteria"/></param>
        /// <param name="searchCriteria">The searchCriteria<see cref="ConfigurationSearchCriteria"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{LocalisationTextDto}}"/></returns>
        Task<PagedQueryResultSet<LocalisationTextDto>> GetAsync(ResultSetCriteria gridCriteria, ConfigurationSearchCriteria searchCriteria);

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="localisationTextDto">The localisationTextDto<see cref="LocalisationTextDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> UpdateAsync(LocalisationTextDto localisationTextDto);
    }
}
