using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Common.Domain.Dto;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IGroupDataContextFactory _groupDataContextFactory;
        private readonly RequestContext _requestContext;
        private readonly IUserContextService _webUserContextService;
        private readonly ISystemDataContext _systemDataContext;

        public ApplicationService(IGroupDataContextFactory groupDataContextFactory, RequestContext requestContext, 
            IUserContextService webUserContextService, ISystemDataContext systemDataContext)
        {
            _groupDataContextFactory = groupDataContextFactory;
            _requestContext = requestContext;
            _webUserContextService = webUserContextService;
            _systemDataContext = systemDataContext;
        }

        public async Task<List<UILocalisationTextDto>> GetUILocalisationTextsForCurrentContextAsync()
        {
            var groups = await GetPermittedGroupIdsForCurrentUserAsync(true);
            var allTexts = new List<UILocalisationTextDto>();

            //Iterate group data stores and collect relevant data.
            foreach (var groupId in groups)
            {
                var dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

                var texts = await dataContext.GetDocumentsAsync<LocalisationText, UILocalisationTextDto>(q =>
                    q.Where(t => t.Culture == _requestContext.Culture || t.Culture == "*")
                    .OrderBy(t => t.Priority)
                    .Select(t => new UILocalisationTextDto
                    {
                        Key = t.Key,
                        GroupId = t.GroupId,
                        Culture = t.Culture,
                        Section = t.Section,
                        Priority = t.Priority,
                        Value = t.Value
                    }));

                allTexts.AddRange(texts);
            }

            allTexts = allTexts.OrderBy(t => t.Priority).ToList();
            return allTexts;
        }

        public async Task<List<ConfigurationDto>> GetConfigurationsForCurrentContextAsync()
        {

            var groups = await GetPermittedGroupIdsForCurrentUserAsync(true);
            var allConfigurations = new List<ConfigurationDto>();

            //Iterate group data stores and collect relevant data.
            foreach (var groupId in groups)
            {
                var dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

                var configurations = await dataContext.GetDocumentsAsync<ConfigurationSetting, ConfigurationDto>(q =>
                                 q.Where(c => c.Culture == _requestContext.Culture || c.Culture == "*")
                                 .OrderBy(t => t.Priority)
                                 .Select(t => new ConfigurationDto
                                 {
                                     Key = t.Key,
                                     GroupId = t.GroupId,
                                     Culture = t.Culture,
                                     Section = t.Section,
                                     Priority = t.Priority,
                                     Value = t.Value
                                 }));
                allConfigurations.AddRange(configurations);
            }
            allConfigurations = allConfigurations.OrderBy(t => t.Priority).ToList();
            return allConfigurations;
        }

        public async Task<List<UILookupDto>> GetUILookupsForCurrentContextAsync()
        {
            var groups = await GetPermittedGroupIdsForCurrentUserAsync(true);
            var allLookups = new List<UILookupDto>();

            //Iterate group data stores and collect relevant data.
            foreach (var groupId in groups)
            {
                var dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

                var lookups = await dataContext.GetDocumentsAsync<Lookup, Lookup>(q =>
                    q.Where(l => l.Culture == _requestContext.Culture || l.Culture == "*")
                    .OrderBy(l => l.Priority));

                var lookupDtos = lookups.Select(t => new UILookupDto
                {
                    Key = t.Key,
                    GroupId = t.GroupId,
                    Culture = t.Culture,
                    Section = t.Section,
                    Priority = t.Priority,
                    Items = t.Items?
                            .OrderBy(i => i, new LookupItemComparer())
                            .Select(i => new LookupItemDto
                            {
                                Value = i.Value,
                                Text = i.Text,
                                Remark = i.Remark,
                                SortOrder = i.SortOrder,
                                ChildLookupKey = i.ChildLookupKey
                            }) ?? new List<LookupItemDto>()
                }).ToList();

                allLookups.AddRange(lookupDtos);
            }

            allLookups = allLookups.OrderBy(t => t.Priority).ToList();
            return allLookups;
        }

        public async Task<List<ListItemDto>> GetPermittedGroupListAsync()
        {
            var groupIds = await GetPermittedGroupIdsForCurrentUserAsync();
            var groupList = await _systemDataContext.GetDocumentsAsync<Group, ListItemDto>(q => 
                                    q.Where(c => groupIds.Contains(c.Id))
                                    .Select(c => new ListItemDto {
                                        Id = c.Id,
                                        Name = c.Name
                                    }));

            if (groupIds.Contains("*"))
            {
                //Add the * group id-name pair manually becuase it's not in the database group list.
                groupList.Add(new ListItemDto
                {
                    Id = "*",
                    Name = "*"
                });
            }

            return groupList.OrderBy(c => c.Name).ToList();
        }

        #region # Mobile Contexts #
        //public async Task<List<UILookupDto>> GetUILocalisationTextsForMobileContextAsync()
        //{
        //    var groups = await GetPermittedGroupIdsForCurrentUserAsync(true);
        //    var allLookups = new List<UILookupDto>();

        //    //Iterate group data stores and collect relevant data.
        //    foreach (var groupId in groups)
        //    {
        //        var dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

        //        var lookups = await dataContext.GetDocumentsAsync<Lookup, Lookup>(q =>
        //            q.Where(l => (l.Culture == _requestContext.Culture || l.Culture == "*") && l.Key == "LIST_LOCALISATIONKEYS_MOBILE"));

        //        var lookupDtos = lookups.Select(t => new UILookupDto
        //        {
        //            Key = t.Key,
        //            GroupId = t.GroupId,
        //            Culture = t.Culture,
        //            Section = t.Section,
        //            Priority = t.Priority,
        //            Items = t.Items?
        //                        .OrderBy(i => i, new LookupItemComparer())
        //                        .Select(i => new LookupItemDto
        //                        {
        //                            Value = i.Value,
        //                            Text = i.Text,
        //                            Remark = i.Remark,
        //                            SortOrder = i.SortOrder,
        //                            ChildLookupKey = i.ChildLookupKey
        //                        }) ?? new List<LookupItemDto>()
        //        }).ToList();

        //        allLookups.AddRange(lookupDtos);
        //    }

        //    allLookups = allLookups.OrderBy(t => t.Priority).ToList();
        //    return allLookups;
        //}

        //public async Task<List<UILookupDto>> GetCountryLookupAsync()
        //{
        //    var groups = await GetPermittedGroupIdsForCurrentUserAsync(true);
        //    var allLookups = new List<UILookupDto>();

        //    //Iterate group data stores and collect relevant data.
        //    foreach (var groupId in groups)
        //    {
        //        var dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

        //        var lookups = await dataContext.GetDocumentsAsync<Lookup, Lookup>(q =>
        //            q.Where(l => (l.Culture == _requestContext.Culture || l.Culture == "*") && l.Key == "LIST_COUNTRY"));

        //        var lookupDtos = lookups.Select(t => new UILookupDto
        //        {
        //            Key = t.Key,
        //            GroupId = t.GroupId,
        //            Culture = t.Culture,
        //            Section = t.Section,
        //            Priority = t.Priority,
        //            Items = t.Items?
        //                        .OrderBy(i => i, new LookupItemComparer())
        //                        .Select(i => new LookupItemDto
        //                        {
        //                            Value = i.Value,
        //                            Text = i.Text,
        //                            Remark = i.Remark,
        //                            SortOrder = i.SortOrder,
        //                            ChildLookupKey = i.ChildLookupKey
        //                        }) ?? new List<LookupItemDto>()
        //        }).ToList();

        //        allLookups.AddRange(lookupDtos);
        //    }

        //    allLookups = allLookups.OrderBy(t => t.Priority).ToList();
        //    return allLookups;
        //}

        //public async Task<List<UILookupDto>> GetMassagePurposeLookupAsync()
        //{
        //    var groups = await GetPermittedGroupIdsForCurrentUserAsync(true);
        //    var allLookups = new List<UILookupDto>();

        //    //Iterate group data stores and collect relevant data.
        //    foreach (var groupId in groups)
        //    {
        //        var dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

        //        var lookups = await dataContext.GetDocumentsAsync<Lookup, Lookup>(q =>
        //            q.Where(l => (l.Culture == _requestContext.Culture || l.Culture == "*") && l.Key == "LIST_MASSAGE_PURPOSE"));

        //        var lookupDtos = lookups.Select(t => new UILookupDto
        //        {
        //            Key = t.Key,
        //            GroupId = t.GroupId,
        //            Culture = t.Culture,
        //            Section = t.Section,
        //            Priority = t.Priority,
        //            Items = t.Items?
        //                        .OrderBy(i => i, new LookupItemComparer())
        //                        .Select(i => new LookupItemDto
        //                        {
        //                            Value = i.Value,
        //                            Text = i.Text,
        //                            Remark = i.Remark,
        //                            SortOrder = i.SortOrder,
        //                            ChildLookupKey = i.ChildLookupKey
        //                        }) ?? new List<LookupItemDto>()
        //        }).ToList();

        //        allLookups.AddRange(lookupDtos);
        //    }

        //    allLookups = allLookups.OrderBy(t => t.Priority).ToList();
        //    return allLookups;
        //}

        public async Task<List<UILookupDto>> GetLookupAsync(string key)
        {
            var groups = await GetPermittedGroupIdsForCurrentUserAsync(true);
            var allLookups = new List<UILookupDto>();
            //Iterate group data stores and collect relevant data.
            foreach (var groupId in groups)
            {
                var dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

                var lookups = await dataContext.GetDocumentsAsync<Lookup, Lookup>(q =>
                    q.Where(l => (l.Culture == _requestContext.Culture || l.Culture == "*") && l.Key==key));

                var lookupDtos = lookups.Select(t => new UILookupDto
                {
                    Key = t.Key,
                    GroupId = t.GroupId,
                    Culture = t.Culture,
                    Section = t.Section,
                    Priority = t.Priority,
                    Items = t.Items?
                                .OrderBy(i => i, new LookupItemComparer())
                                .Select(i => new LookupItemDto
                                {
                                    Value = i.Value,
                                    Text = i.Text,
                                    Remark = i.Remark,
                                    SortOrder = i.SortOrder,
                                    ChildLookupKey = i.ChildLookupKey,
                                    Flag = i.Flag
                                }) ?? new List<LookupItemDto>()
                }).ToList();

                allLookups.AddRange(lookupDtos);
            }

            allLookups = allLookups.OrderBy(t => t.Priority).ToList();
            return allLookups;
        }

        #endregion

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<string>> GetPermissionsForCurrentUserAsync()
        {
            var permissions = (await _webUserContextService.GetClaimsForCurrentUserAsync())
                .Select(c => c);

            return permissions;
        }

        [ExcludeFromCodeCoverage]
        private async Task<IEnumerable<string>> GetPermittedGroupIdsForCurrentUserAsync(bool includeAllOption = false)
        {
            var groups = (await _webUserContextService.GetClaimsForCurrentUserAsync())
                .Select(c => c.Split(':')[0])
                .Distinct();

            return includeAllOption ? groups.Union(new string[] { "*" }) : groups;
        }
    }

    public class LookupItemComparer : IComparer<LookupItem>
    {
        public int Compare(LookupItem x, LookupItem y)
        {
            //Use sortOrder if available. Else, fall back to text comparison.

            if (x.SortOrder != null || y.SortOrder != null)
            {
                var result = GetSortOrder(x).CompareTo(GetSortOrder(y));
                if (result != 0)
                    return result;
            }

            return string.Compare(GetStringComparisonValue(x), GetStringComparisonValue(y));
        }

        private string GetStringComparisonValue(LookupItem item)
        {
            return item.Value == "*" ? item.Value : item.Text;
        }

        private int GetSortOrder(LookupItem item)
        {
            return item.SortOrder == null ? int.MinValue : item.SortOrder.Value;
        }
    }
}
