using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Documents;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    public class LookupReaderService : ILookupReaderService
    {
        private readonly IGroupDataContextFactory _groupDataContextFactory;
        private readonly RequestContext _requestContext;

        public LookupReaderService(IGroupDataContextFactory groupDataContextFactory, RequestContext requestContext)
        {
            _groupDataContextFactory = groupDataContextFactory;
            _requestContext = requestContext;
        }

        public async Task<Lookup> GetLookupAsync(string lookupKey, string groupId = null, string culture = null, string section = null)
        {
            var lookup = await GetLookupFromDbAsync(lookupKey, groupId, culture, section);

            //Fallback to "*" group if no localisation was found for specific group.
            if (lookup == null && groupId != "*")
            {
                lookup = await GetLookupFromDbAsync(lookupKey, "*", culture, section);
            }

            if (lookup != null)
            {
                lookup.Items = lookup.Items.OrderBy(i => i.SortOrder).ThenBy(i => i.Text).ToList();
            }

            return lookup;
        }

        public async Task<string> GetTextAsync(string lookupKey, string value, string groupId = null, string culture = null, string section = null)
        {
            if (string.IsNullOrEmpty(groupId))
                groupId = _requestContext.GroupId;

            if (string.IsNullOrEmpty(section))
                section = "*";

            if (string.IsNullOrEmpty(culture))
                culture = _requestContext.Culture;

            var lookup = await GetLookupAsync(lookupKey, groupId, culture, section);

            var item = lookup.Items.FirstOrDefault(i => i.Value == value);

            return item?.Text;
        }

        private async Task<Lookup> GetLookupFromDbAsync(string key, string groupId, string culture, string section)
        {
            IGroupDataContext dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

            var lookup = await dataContext.FirstOrDefaultAsync<Lookup, Lookup>(q =>
                                q.Where(c =>
                                    (c.Culture == culture || c.Culture == "*")
                                    && (c.Section == section || c.Section == "*")
                                    && c.Key == key)
                                .OrderBy(c => c.Priority));

            return lookup;
        }
    }
}
