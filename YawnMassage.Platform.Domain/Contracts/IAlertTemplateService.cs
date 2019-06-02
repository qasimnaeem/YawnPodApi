using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Platform.Domain.Dto.Configuration;

namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IAlertTemplateService" />
    /// </summary>
    public interface IAlertTemplateService
    {
        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="dto">The dto<see cref="AlertTemplateDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateAsync(AlertTemplateDto dto);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="ids">The ids<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(params string[] ids);

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="gridCriteria">The gridCriteria<see cref="ResultSetCriteria"/></param>
        /// <param name="searchCriteria">The searchCriteria<see cref="AlertTemplateSearchCriteria"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{AlertTemplateDto}}"/></returns>
        Task<PagedQueryResultSet<AlertTemplateDto>> GetAsync(ResultSetCriteria gridCriteria, AlertTemplateSearchCriteria searchCriteria);

        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{AlertTemplateDto}"/></returns>
        Task<AlertTemplateDto> GetAsync(string id);

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="dto">The dto<see cref="AlertTemplateDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> UpdateAsync(AlertTemplateDto dto);
    }
}
