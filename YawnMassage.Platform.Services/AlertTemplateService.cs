using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Platform.Services.Helpers;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class AlertTemplateService : IAlertTemplateService
    {
        private readonly IGroupDataContext _dataContext;

        public AlertTemplateService(IGroupDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<DocumentUpdateResultDto> CreateAsync(AlertTemplateDto dto)
        {
            ValidateDto(dto);
            await CheckForDuplicatesAsync(dto);

            AlertTemplate alertTemplate = new AlertTemplate();
            AssignDtoToEntity(alertTemplate, dto);
            return await _dataContext.CreateDocumentAsync(alertTemplate);
        }

        public async Task<DocumentUpdateResultDto> UpdateAsync(AlertTemplateDto dto)
        {
            ValidateDto(dto);
            await CheckForDuplicatesAsync(dto);

            var localisation = await GetAlertTemplateAsync(dto.Id);

            AssignDtoToEntity(localisation, dto);
            return await _dataContext.ReplaceDocumentAsync(localisation);
        }

        [ExcludeFromCodeCoverage]
        public async Task DeleteAsync(params string[] ids)
        {
            await _dataContext.SoftDeleteDocumentsAsync<AlertTemplate>(ids);
        }

        public async Task<AlertTemplateDto> GetAsync(string id)
        {
            var alertTemplate = await GetAlertTemplateAsync(id);
            
            AlertTemplateDto dto = new AlertTemplateDto()
            {
                Id = alertTemplate.Id,
                ETag = alertTemplate.ETag,
                GroupId = alertTemplate.GroupId,
                Culture = alertTemplate.Culture,
                Section = alertTemplate.Section,
                Key = alertTemplate.Key,
                Priority = alertTemplate.Priority,
                Channel = alertTemplate.Channel,
                SenderId = alertTemplate.SenderId,
                Subject = alertTemplate.Subject,
                Content = alertTemplate.Content,
                Remark = alertTemplate.Remark,
                UpdatedOnUtc = alertTemplate.UpdatedOnUtc,
                UpdatedById = alertTemplate.UpdatedById,
                UpdatedByName = alertTemplate.UpdatedByName
            };
            return dto;
        }

        public async Task<PagedQueryResultSet<AlertTemplateDto>> GetAsync(ResultSetCriteria gridCriteria, AlertTemplateSearchCriteria searchCriteria)
        {
            if (string.IsNullOrWhiteSpace(searchCriteria.Key))
                searchCriteria.Key = "";

            var result = await _dataContext.GetDocumentsWithPagingAsync<AlertTemplate, AlertTemplateDto>(q =>
                  q.Where(d =>
                    (searchCriteria.Culture == "any" || d.Culture == searchCriteria.Culture)
                    && (searchCriteria.Section == "any" || d.Section == searchCriteria.Section)
                    && (searchCriteria.Key == "" || d.Key.Contains(searchCriteria.Key.ToUpper().Trim()))
                    && (searchCriteria.Channel == "any" || d.Channel == searchCriteria.Channel))
                    .Select(c => new AlertTemplateDto
                    {
                        Id = c.Id,
                        IsDeleted = c.IsDeleted,
                        Culture = c.Culture,
                        GroupId = c.GroupId,
                        Section = c.Section,
                        Key = c.Key,
                        Priority = c.Priority,
                        Channel = c.Channel,
                        ETag = c.ETag,
                        UpdatedOnUtc = c.UpdatedOnUtc,
                        UpdatedById = c.UpdatedById,
                        UpdatedByName = c.UpdatedByName
                    }), gridCriteria);

            return result;
        }

        private async Task<AlertTemplate> GetAlertTemplateAsync(string id)
        {
            return await _dataContext.GetDocumentAsync<AlertTemplate>(id);
        }

        private void AssignDtoToEntity(AlertTemplate alertTemplate, AlertTemplateDto dto)
        {
            alertTemplate.Section = dto.Section;
            alertTemplate.Culture = dto.Culture;
            alertTemplate.Key = dto.Key;
            alertTemplate.Channel = dto.Channel;
            alertTemplate.SenderId = dto.SenderId;
            alertTemplate.Subject = dto.Subject;
            alertTemplate.Content = dto.Content;
            alertTemplate.Remark = dto.Remark;
            alertTemplate.Priority = ConfigurationHelper.GetConfigurationPriority(dto.GroupId, dto.Culture, dto.Section);

            if (!string.IsNullOrEmpty(alertTemplate.Id))
            {
                alertTemplate.ETag = dto.ETag;
            }
        }

        private async Task CheckForDuplicatesAsync(AlertTemplateDto dto)
        {
            var exists = await _dataContext.AnyAsync<AlertTemplate>(q => q.Where(d =>
                    d.Key == dto.Key
                    && d.GroupId == dto.GroupId
                    && d.Culture == dto.Culture
                    && d.Section == dto.Section
                    && d.Channel == dto.Channel
                    && (dto.Id == null || d.Id != dto.Id)));

            if (exists)
                throw new DocumentDuplicateException();
        }

        private void ValidateDto(AlertTemplateDto dto)
        {
            if ((dto.Channel == SystemKeys.AlertChannelKeys.Email && string.IsNullOrEmpty(dto.Subject)) 
                || (dto.Channel != SystemKeys.AlertChannelKeys.Email && !string.IsNullOrEmpty(dto.Subject)))
            {
                throw new YawnMassageException("UNSUPPORTED_FIELDS_FOR_CHANNEL");
            }
        }
    }
}
