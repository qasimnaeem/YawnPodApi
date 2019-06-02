using YawnMassage.Common.Domain.Dto.ResultSet;
using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Platform.Domain.Dto.ConfigurationSetting;
using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IConfigurationService" />
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="gridCriteria">The gridCriteria<see cref="ResultSetCriteria"/></param>
        /// <param name="searchCriteria">The searchCriteria<see cref="ConfigurationSearchCriteria"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{ConfigurationSettingDto}}"/></returns>
        Task<PagedQueryResultSet<ConfigurationSettingDto>> GetAsync(ResultSetCriteria gridCriteria, ConfigurationSearchCriteria searchCriteria);

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{ConfigurationSettingDto}"/></returns>
        Task<ConfigurationSettingDto> GetAsync(string id);

        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="configurationSettingDto">The configurationSettingDto<see cref="ConfigurationSettingDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateAsync(ConfigurationSettingDto configurationSettingDto);

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="configurationSettingDto">The configurationSettingDto<see cref="ConfigurationSettingDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> UpdateAsync(ConfigurationSettingDto configurationSettingDto);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="ids">The ids<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(params string[] ids);
    }
}
