using System.Threading.Tasks;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Platform.Domain.Dto.Configuration;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ILookupService" />
    /// </summary>
    public interface ILookupService
    {
        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{LookupDto}"/></returns>
        Task<LookupDto> GetAsync(string id);

        /// <summary>
        /// The GetLookupListAsync
        /// </summary>
        /// <param name="gridCriteria">The gridCriteria<see cref="ResultSetCriteria"/></param>
        /// <param name="searchCriteria">The searchCriteria<see cref="ConfigurationSearchCriteria"/></param>
        /// <returns>The <see cref="Task{PagedQueryResultSet{LookupDto}}"/></returns>
        Task<PagedQueryResultSet<LookupDto>> GetLookupListAsync(ResultSetCriteria gridCriteria, ConfigurationSearchCriteria searchCriteria);

        /// <summary>
        /// The CreateAsync
        /// </summary>
        /// <param name="lookupDto">The lookupDto<see cref="LookupDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> CreateAsync(LookupDto lookupDto);

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="lookupDto">The lookupDto<see cref="LookupDto"/></param>
        /// <returns>The <see cref="Task{DocumentUpdateResultDto}"/></returns>
        Task<DocumentUpdateResultDto> UpdateAsync(LookupDto lookupDto);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="ids">The ids<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(params string[] ids);

        /// <summary>
        /// The GetLookupListByKeyAsync
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="Task{Lookup}"/></returns>
        Task<Lookup> GetLookupListByKeyAsync(string key);
    }
}
