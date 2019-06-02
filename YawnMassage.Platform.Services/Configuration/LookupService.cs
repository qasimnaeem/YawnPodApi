using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Platform.Services.Helpers;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Exceptions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class LookupService : ILookupService
    {
        private readonly IGroupDataContext _dataContext;
        private readonly ISystemDataContext _systemDataContext;
        private readonly RequestContext _requestContext;
        private readonly IGroupDataContextFactory _groupDataContextFactory;

        public LookupService(IGroupDataContext dataContext, ISystemDataContext systemDataContext, 
            RequestContext requestContext, IGroupDataContextFactory groupDataContextFactory)
        {
            _dataContext = dataContext;
            _systemDataContext = systemDataContext;
            _requestContext = requestContext;
            _groupDataContextFactory = groupDataContextFactory;
        }

        public async Task<DocumentUpdateResultDto> CreateAsync(LookupDto dto)
        {
            await CheckForDuplicates(dto.GroupId, dto.Culture, dto.Section, dto.Key);

            if(dto.Items != null)
                CheckForCircularReference(dto.Key, dto.Items);

            var lookup = new Lookup
            {
                Culture = dto.Culture,
                Section = dto.Section,
                Key = dto.Key,
                IncludeInPod = dto.IncludeInPod,
                Priority = ConfigurationHelper.GetConfigurationPriority(dto.GroupId, dto.Culture, dto.Section),
                Items = CreateLookupItems(dto.Items),
            };

            var result = await _dataContext.CreateDocumentAsync(lookup);

            return result;
        }

        [ExcludeFromCodeCoverage]
        public async Task DeleteAsync(params string[] ids)
        {
            await _dataContext.SoftDeleteDocumentsAsync<Lookup>(ids);
        }

        public async Task<LookupDto> GetAsync(string id)
        {
            var lookup = await _dataContext.GetDocumentAsync<Lookup>(id);

            if (lookup == null)
                return null;

            var dto = new LookupDto
            {
                Id = lookup.Id,
                IsDeleted = lookup.IsDeleted,
                Culture = lookup.Culture,
                GroupId = lookup.GroupId,
                Key = lookup.Key,
                IncludeInPod = lookup.IncludeInPod,
                Section = lookup.Section,
                Priority = lookup.Priority,
                UpdatedById = lookup.UpdatedById,
                UpdatedByName = lookup.UpdatedByName,
                UpdatedOnUtc = lookup.UpdatedOnUtc,
                ETag = lookup.ETag,
                Items = lookup.Items?
                    .Select(v => new LookupItemDto
                    {
                        Text = v.Text,
                        Value = v.Value,
                        Remark = v.Remark,
                        SortOrder = v.SortOrder,
                        ChildLookupKey = v.ChildLookupKey
                    }).ToList()
            };

            return dto;
        }

        public async Task<PagedQueryResultSet<LookupDto>> GetLookupListAsync(ResultSetCriteria gridCriteria, ConfigurationSearchCriteria searchCriteria)
        {
            if (string.IsNullOrWhiteSpace(searchCriteria.Key))
                searchCriteria.Key = "";

            var result = await _dataContext.GetDocumentsWithPagingAsync<Lookup, Lookup>(q =>
                  q.Where(d =>
                    (searchCriteria.Culture == "any" || d.Culture == searchCriteria.Culture)
                    && (searchCriteria.Section == "any" || d.Section == searchCriteria.Section)
                    && (searchCriteria.Key == "" || d.Key.Contains(searchCriteria.Key.ToUpper().Trim()))),
                    gridCriteria);


            var lookupDtoList = result.Results.Select(l => new LookupDto
            {
                Id = l.Id,
                IsDeleted = l.IsDeleted,
                Culture = l.Culture,
                GroupId = l.GroupId,
                Key = l.Key,
                IncludeInPod = l.IncludeInPod,
                Section = l.Section,
                Priority = l.Priority,
                UpdatedById = l.UpdatedById,
                UpdatedByName = l.UpdatedByName,
                UpdatedOnUtc = l.UpdatedOnUtc,
                ItemValueList = l.Items.OrderBy(i=> i.Value).Take(6).Select(item => item.Value)
            }).ToList();

            return new PagedQueryResultSet<LookupDto> { Results = lookupDtoList, ContinuationToken = result.ContinuationToken };
        }
        
        public async Task<DocumentUpdateResultDto> UpdateAsync(LookupDto dto)
        {
            var lookup = await _dataContext.GetDocumentAsync<Lookup>(dto.Id);
            
            await CheckForDuplicates(dto.GroupId, dto.Culture, dto.Section, dto.Key, dto.Id);

            if (dto.Items != null)
                CheckForCircularReference(dto.Key, dto.Items);

            lookup.Culture = dto.Culture;
            lookup.Section = dto.Section;
            lookup.Key = dto.Key;
            lookup.IncludeInPod = dto.IncludeInPod;
            lookup.Priority = ConfigurationHelper.GetConfigurationPriority(dto.GroupId, dto.Culture, dto.Section);
            lookup.Items = CreateLookupItems(dto.Items);
            lookup.ETag = dto.ETag;

            var result = await _dataContext.ReplaceDocumentAsync(lookup);

            return result;
        }

        public async Task<Lookup> GetLookupListByKeyAsync(string key)
        {
            var lookup = await _dataContext.FirstOrDefaultAsync<Lookup, Lookup>(q => q.Where(l => l.Key == key));

            if (lookup == null)
                lookup = await _systemDataContext.FirstOrDefaultAsync<Lookup, Lookup>(q => q.Where(l => l.Key == key));

            return lookup;
        }

        private List<LookupItem> CreateLookupItems(List<LookupItemDto> lookupItemDtos)
        {
            var data = lookupItemDtos?.Select(l => new LookupItem
            {
                Text = l.Text,
                Value = l.Value,
                Remark = l.Remark,
                SortOrder = l.SortOrder,
                ChildLookupKey = l.ChildLookupKey
            }).ToList();

            return data ?? new List<LookupItem>();
        }

        private async Task CheckForDuplicates(string group, string culture, string section, string key, string selfId = null)
        {
            var exists = await _dataContext.AnyAsync<Lookup>(q => q.Where(l => l.Key == key
                            && !l.IsDeleted
                            && l.GroupId == group
                            && l.Culture == culture
                            && l.Section == section
                            && (selfId == null || l.Id != selfId)));

            if (exists)
                throw new DocumentDuplicateException();
        }

        private void CheckForCircularReference(string lookupKey, List<LookupItemDto> lookupItems)
        {
            foreach (var item in lookupItems)
            {
                if (item.ChildLookupKey == lookupKey)
                    throw new YawnMassageException("ERROR_LOOKUP_CIRCULAR_REFERENCE");
            }
        }
    }
}
