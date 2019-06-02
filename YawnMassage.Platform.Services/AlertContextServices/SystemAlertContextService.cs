using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.AlertContextServices
{
    public class SystemAlertContextService : IAlertContextObjectService
    {
        private readonly ILocalisationReaderService _localisationReaderService;
        private readonly IConfigurationReaderService _configurationReaderService;

        public SystemAlertContextService(ILocalisationReaderService localisationReaderService, IConfigurationReaderService configurationReaderService)
        {
            _localisationReaderService = localisationReaderService;
            _configurationReaderService = configurationReaderService;
        }

        public async Task<Dictionary<string, string>> GetContextObjectKeyValuesAsync(object objectInfo, string groupId, string culture, string timeZone)
        {
            var productName = await _localisationReaderService.GetTextAsync(SystemKeys.LocalisationKeys.ProductName, groupId, culture);
            var companyName = await _localisationReaderService.GetTextAsync(SystemKeys.LocalisationKeys.CompanyName, groupId, culture);
            var logoUrl = await _configurationReaderService.GetValueAsync(SystemKeys.ConfigurationKeys.SystemLogoUrl, groupId, culture);

            var dic = new Dictionary<string, string>
            {
                { "productName", productName },
                { "companyName", companyName },
                { "logoUrl", logoUrl }
            };

            return dic;
        }
    }
}
