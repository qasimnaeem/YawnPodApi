using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Platform.Domain.Dto.Configuration;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IApplicationService" />
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// The GetUILocalisationTextsForCurrentContextAsync
        /// </summary>
        /// <returns>The <see cref="Task{List{UILocalisationTextDto}}"/></returns>
        Task<List<UILocalisationTextDto>> GetUILocalisationTextsForCurrentContextAsync();

        /// <summary>
        /// The GetUILookupsForCurrentContextAsync
        /// </summary>
        /// <returns>The <see cref="Task{List{UILookupDto}}"/></returns>
        Task<List<UILookupDto>> GetUILookupsForCurrentContextAsync();

        /// <summary>
        /// The GetConfigurationsForCurrentContextAsync
        /// </summary>
        /// <returns>The <see cref="Task{List{ConfigurationDto}}"/></returns>
        Task<List<ConfigurationDto>> GetConfigurationsForCurrentContextAsync();

        /// <summary>
        /// The GetPermittedGroupListAsync
        /// </summary>
        /// <returns>The <see cref="Task{List{ListItemDto}}"/></returns>
        Task<List<ListItemDto>> GetPermittedGroupListAsync();

        /// <summary>
        /// The GetPermissionsForCurrentUserAsync
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{string}}"/></returns>
        Task<IEnumerable<string>> GetPermissionsForCurrentUserAsync();

        //Task<List<UILookupDto>> GetUILocalisationTextsForMobileContextAsync();

        //Task<List<UILookupDto>> GetCountryLookupAsync();
        //Task<List<UILookupDto>> GetMassagePurposeLookupAsync();
        Task<List<UILookupDto>> GetLookupAsync(string key);
    }
}
