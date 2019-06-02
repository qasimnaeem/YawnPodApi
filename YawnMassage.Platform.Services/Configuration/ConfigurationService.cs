using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Platform.Domain.Dto.ConfigurationSetting;
using YawnMassage.Platform.Services.Helpers;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IGroupDataContext _dataContext;
        private readonly IGroupDataContextFactory _groupDataContextFactory;
        public ConfigurationService(IGroupDataContextFactory groupDataContextFactory, IGroupDataContext dataContext)
        {
            _groupDataContextFactory = groupDataContextFactory;
            _dataContext = dataContext;
        }

        public async Task<PagedQueryResultSet<ConfigurationSettingDto>> GetAsync(ResultSetCriteria gridCriteria, ConfigurationSearchCriteria searchCriteria)
        {
            string key = searchCriteria.Key ?? string.Empty;

            var configurationList = (await _dataContext.GetDocumentsWithPagingAsync<ConfigurationSetting, ConfigurationSettingDto>
                                    (q => q.Where(c =>
                                        (searchCriteria.Culture == "any" || c.Culture == searchCriteria.Culture)
                                        && (searchCriteria.Section == "any" || c.Section == searchCriteria.Section)
                                        && (string.IsNullOrEmpty(key) || c.Key.ToLower().Contains(key.ToLower().Trim())))
                                        .Select(c => new ConfigurationSettingDto
                                        {
                                            Id = c.Id,
                                            IsDeleted = c.IsDeleted,
                                            Culture = c.Culture,
                                            GroupId = c.GroupId,
                                            Section = c.Section,
                                            Key = c.Key,
                                            IncludeInPod = c.IncludeInPod,
                                            Priority = c.Priority,
                                            Value = c.Value, 
                                            ETag = c.ETag,
                                            UpdatedOnUtc = c.UpdatedOnUtc,
                                            UpdatedByName = c.UpdatedByName,
                                            UpdatedById = c.UpdatedById
                                        }), gridCriteria));

            return configurationList;
        }

        public async Task<ConfigurationSettingDto> GetAsync(string id)
        {
            var configSetting = await GetConfigurationSettingAsync(id);

            if (configSetting == null)
                return null;

            ConfigurationSettingDto configurationSettingDto = new ConfigurationSettingDto()
            {
                Id = configSetting.Id,
                ETag = configSetting.ETag,
                GroupId = configSetting.GroupId,
                Culture = configSetting.Culture,
                Section = configSetting.Section,
                Key = configSetting.Key,
                Remark = configSetting.Remark,
                IncludeInPod = configSetting.IncludeInPod,
                Priority = configSetting.Priority,
                Value = configSetting.Value,
                UpdatedOnUtc = configSetting.UpdatedOnUtc,
                UpdatedById = configSetting.UpdatedById,
                UpdatedByName = configSetting.UpdatedByName
            };
            return configurationSettingDto;
        }

        public async Task<DocumentUpdateResultDto> CreateAsync(ConfigurationSettingDto configurationSettingDto)
        {
            await ValidateConfigurationSettingAsync(configurationSettingDto);
            ConfigurationSetting configurationSetting = new ConfigurationSetting();
            AssignConfigurationDtoToEntity(configurationSetting, configurationSettingDto);
            return await _dataContext.CreateDocumentAsync(configurationSetting);
        }

        public async Task<DocumentUpdateResultDto> UpdateAsync(ConfigurationSettingDto configurationSettingDto)
        {
            await ValidateConfigurationSettingAsync(configurationSettingDto);

            var configuration = await GetConfigurationSettingAsync(configurationSettingDto.Id);

            AssignConfigurationDtoToEntity(configuration, configurationSettingDto);
            return await _dataContext.ReplaceDocumentAsync(configuration);
        }

        [ExcludeFromCodeCoverage]
        public async Task DeleteAsync(params string[] ids)
        {
            await _dataContext.SoftDeleteDocumentsAsync<ConfigurationSetting>(ids);
        }

        private async Task<ConfigurationSetting> GetConfigurationSettingAsync(string id)
        {
            return await _dataContext.GetDocumentAsync<ConfigurationSetting>(id);
        }

        private void AssignConfigurationDtoToEntity(ConfigurationSetting configurationSetting, ConfigurationSettingDto configurationSettingDto)
        {
            configurationSetting.Section = configurationSettingDto.Section;
            configurationSetting.Culture = configurationSettingDto.Culture;
            configurationSetting.Key = configurationSettingDto.Key;
            configurationSetting.IncludeInPod = configurationSettingDto.IncludeInPod;
            configurationSetting.Value = configurationSettingDto.Value;
            configurationSetting.Remark = configurationSettingDto.Remark;
            configurationSetting.Priority = ConfigurationHelper.GetConfigurationPriority(configurationSettingDto.GroupId, configurationSettingDto.Culture, configurationSettingDto.Section);
            if (!string.IsNullOrEmpty(configurationSetting.Id))
            {
                configurationSetting.ETag = configurationSettingDto.ETag;
            }
        }

        private async Task ValidateConfigurationSettingAsync(ConfigurationSettingDto configuration)
        {
            if (string.IsNullOrEmpty(configuration.Section))
            {
                throw new YawnMassageException("ERR_SECTION_CANNOT_BE_NULL");
            }

            if (string.IsNullOrEmpty(configuration.Culture))
            {
                throw new YawnMassageException("ERR_CULTURE_CANNOT_BE_NULL");
            }

            if (string.IsNullOrEmpty(configuration.Value))
            {
                throw new YawnMassageException("ERR_VALUE_CANNOT_BE_NULL");
            }

            if (string.IsNullOrEmpty(configuration.Key))
            {
                throw new YawnMassageException("ERR_KEY_CANNOT_BE_NULL");
            }

            var exists = await _dataContext.AnyAsync<ConfigurationSetting>(q => q.Where(d =>
                    d.Key == configuration.Key
                    && d.GroupId == configuration.GroupId
                    && d.Culture == configuration.Culture
                    && d.Section == configuration.Section
                    && (configuration.Id == null || d.Id != configuration.Id)));

            if (exists)
                throw new DocumentDuplicateException();
        }
    }
}
