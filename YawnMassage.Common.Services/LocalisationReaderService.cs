using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Documents;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    public class LocalisationReaderService : ILocalisationReaderService
    {
        private readonly IGroupDataContextFactory _groupDataContextFactory;
        private readonly RequestContext _requestContext;

        public LocalisationReaderService(IGroupDataContextFactory groupDataContextFactory, RequestContext requestContext)
        {
            _groupDataContextFactory = groupDataContextFactory;
            _requestContext = requestContext;
        }

        public async Task<string> GetTextAsync(string localisationKey, string groupId = null, string culture = null, string section = null)
        {
            if (string.IsNullOrEmpty(groupId))
                groupId = _requestContext.GroupId;

            if (string.IsNullOrEmpty(section))
                section = "*";

            if (string.IsNullOrEmpty(culture))
                culture = _requestContext.Culture;

            var text = await GetLocalisationTextAsync(localisationKey, groupId, culture, section);

            //Fallback to "*" group if no localisation was found for specific group.
            if (text == null && groupId != "*")
            {
                text = await GetLocalisationTextAsync(localisationKey, "*", culture, section);
            }

            return text?.Value ?? localisationKey;
        }

        private async Task<LocalisationText> GetLocalisationTextAsync(string key, string groupId, string culture, string section)
        {
            IGroupDataContext dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);

            var text = await dataContext.FirstOrDefaultAsync<LocalisationText, LocalisationText>(q =>
                                q.Where(c =>
                                    (c.Culture == culture || c.Culture == "*")
                                    && (c.Section == section || c.Section == "*")
                                    && c.Key == key)
                                .OrderBy(c => c.Priority));

            return text;
        }
    }
}
