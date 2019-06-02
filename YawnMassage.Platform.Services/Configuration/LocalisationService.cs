using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Platform.Services.Helpers;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class LocalisationService : ILocalisationService
    {
        private readonly IGroupDataContext _dataContext;
        private readonly RequestContext _requestContext;

        public LocalisationService(IGroupDataContext dataContext, RequestContext requestContext)
        {
            _dataContext = dataContext;
            _requestContext = requestContext;
        }

        public async Task<DocumentUpdateResultDto> CreateAsync(LocalisationTextDto localisationTextDto)
        {
            await CheckForDuplicates(localisationTextDto);

            LocalisationText localisationText = new LocalisationText();
            AssignLocalisationDtoToEntity(localisationText, localisationTextDto);
            return await _dataContext.CreateDocumentAsync(localisationText);
        }

        [ExcludeFromCodeCoverage]
        public async Task DeleteAsync(params string[] ids)
        {
            await _dataContext.SoftDeleteDocumentsAsync<LocalisationText>(ids);
        }

        public async Task<LocalisationTextDto> GetAsync(string id)
        {
            var localisationText = await GetLocalisationTextAsync(id);

            if (localisationText == null)
                return null;

            LocalisationTextDto localisationTextDto = new LocalisationTextDto()
            {
                Id = localisationText.Id,
                ETag = localisationText.ETag,
                GroupId = localisationText.GroupId,
                Culture = localisationText.Culture,
                Section = localisationText.Section,
                Key = localisationText.Key,
                Remark = localisationText.Remark,
                Priority = localisationText.Priority,
                Value = localisationText.Value,
                IncludeInPod = localisationText.IncludeInPod,
                UpdatedOnUtc = localisationText.UpdatedOnUtc,
                UpdatedById = localisationText.UpdatedById,
                UpdatedByName = localisationText.UpdatedByName
            };
            return localisationTextDto;
        }

        public async Task<PagedQueryResultSet<LocalisationTextDto>> GetAsync(ResultSetCriteria gridCriteria, ConfigurationSearchCriteria searchCriteria)
        {
            if (string.IsNullOrWhiteSpace(searchCriteria.Key))
                searchCriteria.Key = "";

            var result = await _dataContext.GetDocumentsWithPagingAsync<LocalisationText, LocalisationTextDto>(q =>
                  q.Where(d =>
                    (searchCriteria.Culture == "any" || d.Culture == searchCriteria.Culture)
                    && (searchCriteria.Section == "any" || d.Section == searchCriteria.Section)
                    && (searchCriteria.Key == "" || d.Key.Contains(searchCriteria.Key.ToUpper().Trim())))
                    .Select(c => new LocalisationTextDto
                    {
                        Id = c.Id,
                        IsDeleted = c.IsDeleted,
                        Culture = c.Culture,
                        GroupId = c.GroupId,
                        Section = c.Section,
                        Key = c.Key,
                        Priority = c.Priority,
                        Value = c.Value,
                        IncludeInPod = c.IncludeInPod,
                        ETag = c.ETag,
                        UpdatedOnUtc = c.UpdatedOnUtc,
                        UpdatedById = c.UpdatedById,
                        UpdatedByName = c.UpdatedByName
                    }), gridCriteria);

            return result;
        }

        public async Task<DocumentUpdateResultDto> UpdateAsync(LocalisationTextDto localisationTextDto)
        {
            await CheckForDuplicates(localisationTextDto);

            var localisation = await GetLocalisationTextAsync(localisationTextDto.Id);

            AssignLocalisationDtoToEntity(localisation, localisationTextDto);
            return await _dataContext.ReplaceDocumentAsync(localisation);
        }
        
        private async Task<LocalisationText> GetLocalisationTextAsync(string id)
        {
            return await _dataContext.GetDocumentAsync<LocalisationText>(id);
        }

        private void AssignLocalisationDtoToEntity(LocalisationText localisationSetting, LocalisationTextDto localisationSettingDto)
        {
            localisationSetting.Section = localisationSettingDto.Section;
            localisationSetting.Culture = localisationSettingDto.Culture;
            localisationSetting.Key = localisationSettingDto.Key;
            localisationSetting.Remark = localisationSettingDto.Remark;
            localisationSetting.Value = localisationSettingDto.Value;
            localisationSetting.IncludeInPod = localisationSettingDto.IncludeInPod;
            localisationSetting.Priority = ConfigurationHelper.GetConfigurationPriority(localisationSettingDto.GroupId, localisationSettingDto.Culture, localisationSettingDto.Section);
            if (!string.IsNullOrEmpty(localisationSetting.Id))
            {
                localisationSetting.ETag = localisationSettingDto.ETag;
            }
        }

        private async Task CheckForDuplicates(LocalisationTextDto localisationTextDto)
        {
            var exists = await _dataContext.AnyAsync<LocalisationText>(q => q.Where(d =>
                    d.Key == localisationTextDto.Key
                    && d.GroupId == localisationTextDto.GroupId
                    && d.Culture == localisationTextDto.Culture
                    && d.Section == localisationTextDto.Section
                    && (localisationTextDto.Id == null || d.Id != localisationTextDto.Id)));

            if (exists)
                throw new DocumentDuplicateException();
        }


    }
}
