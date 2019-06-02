using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.DeviceKeyNormalisation;
using YawnMassage.Platform.Domain.Dto.DeviceNormalisation;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class PlatformNormalisationService : IPlatformNormalisationService
    {
        private readonly ISystemDataContext _systemDataContext;
        private readonly IGroupDataContext _groupDataContext;

        public PlatformNormalisationService(ISystemDataContext systemDataContext, IGroupDataContext groupDataContext)
        {
            _systemDataContext = systemDataContext;
            _groupDataContext = groupDataContext;
        }

        public async Task<List<LocalisationDto>> NormaliseLocalisationAsync()
        {
            var systemLocalisationTextList = await GetLocalisationsAsync(_systemDataContext);
            var groupLocalisationTextList = await GetLocalisationsAsync(_groupDataContext);

            var commonList = groupLocalisationTextList.Intersect(systemLocalisationTextList, new NormalisationDataComparer());

            var excludedFromSystem = systemLocalisationTextList.Where(ah => !commonList.Any(h => h.Key == ah.Key
                                                       && h.Section == ah.Section
                                                       && h.Culture == ah.Culture)).ToList();

            var mergedList = excludedFromSystem.Union(groupLocalisationTextList).ToList();

            return mergedList;
        }

        public async Task<List<LookupDto>> NormaliseLookupAsync()
        {
            var systemLookupList = await GetLookupsAsync(_systemDataContext);

            var groupLookupList = await GetLookupsAsync(_groupDataContext);

            var commonList = groupLookupList.Intersect(systemLookupList, new NormalisationDataComparer());

            var excludedFromSystem = systemLookupList.Where(ah => !commonList.Any(h => h.Key == ah.Key
                                                      && h.Section == ah.Section
                                                      && h.Culture == ah.Culture)).ToList();

            var mergedList = excludedFromSystem.Union(groupLookupList).ToList();

            return mergedList;
        }

        private async Task<IEnumerable<LocalisationDto>> GetLocalisationsAsync(IAuditedDocumentDbService auditedDocumentDbService)
        {
            var localisationTextList = await auditedDocumentDbService.GetDocumentsAsync<LocalisationText, LocalisationDto>
                (q => q.Where(l => l.IncludeInPod).Select(l => new LocalisationDto
                {
                    Culture = l.Culture,
                    Key = l.Key,
                    Section = l.Section,
                    Value = l.Value
                }));

            return localisationTextList;
        }

        private async Task<IEnumerable<LookupDto>> GetLookupsAsync(IAuditedDocumentDbService auditedDocumentDbService)
        {
            var lookups = await auditedDocumentDbService.GetDocumentsAsync<Lookup, Lookup>
               (q => q.Where(l => l.IncludeInPod));

            var dtoList = lookups.Select(l => new LookupDto()
            {
                Culture = l.Culture,
                Key = l.Key,
                Section = l.Section,
                Values = l.Items?.Select(i => new LookupValueDto
                {
                    ChildLookupKey = i.ChildLookupKey,
                    SortOrder = i.SortOrder,
                    Text = i.Text,
                    Value = i.Value
                }) ?? new List<LookupValueDto>()
            });

            return dtoList;
        }
    }

    internal class NormalisationDataComparer : IEqualityComparer<BaseNormalisationDto>
    {
        public bool Equals(BaseNormalisationDto groupSpecificList, BaseNormalisationDto systemSpecificList)
        {
            if (groupSpecificList.Key == systemSpecificList.Key && groupSpecificList.Section == systemSpecificList.Section
                && groupSpecificList.Culture == systemSpecificList.Culture)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(BaseNormalisationDto obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
