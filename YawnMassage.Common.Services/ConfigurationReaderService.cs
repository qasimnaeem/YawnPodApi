using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Documents;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    public class ConfigurationReaderService : IConfigurationReaderService
    {
        private readonly IGroupDataContextFactory _groupDataContextFactory;
        private readonly RequestContext _requestContext;

        public ConfigurationReaderService(IGroupDataContextFactory groupDataContextFactory, RequestContext requestContext)
        {
            _groupDataContextFactory = groupDataContextFactory;
            _requestContext = requestContext;
        }

        public async Task<string> GetValueAsync(string configurationKey, string groupId = null, string culture = null, string section = null)
        {
            if (string.IsNullOrEmpty(groupId))
                groupId = _requestContext.GroupId;

            if (string.IsNullOrEmpty(section))
                section = "*";

            if (string.IsNullOrEmpty(culture))
                culture = _requestContext.Culture;

            var config = await GetConfigurationSettingAsync(configurationKey, groupId, culture, section);

            //Fallback to "*" group if no config was found for specific group.
            if (config == null && groupId != "*")
            {
                config = await GetConfigurationSettingAsync(configurationKey, "*", culture, section);
            }

            return config?.Value;
        }

        private async Task<ConfigurationSetting> GetConfigurationSettingAsync(string key, string groupId, string culture, string section)
        {
            IGroupDataContext dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

            var config = await dataContext.FirstOrDefaultAsync<ConfigurationSetting, ConfigurationSetting>(q =>
                                q.Where(c =>
                                    (c.Culture == culture || c.Culture == "*")
                                    && (c.Section == section || c.Section == "*")
                                    && c.Key == key)
                                .OrderBy(c => c.Priority));

            return config;
        }
    }
}
