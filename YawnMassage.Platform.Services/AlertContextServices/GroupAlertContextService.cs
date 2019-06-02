using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Domain.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.AlertContextServices
{
    public class GroupAlertContextService : IAlertContextObjectService
    {
        private readonly ISystemDataContext _systemDataContext;

        public GroupAlertContextService(ISystemDataContext systemDataContext)
        {
            _systemDataContext = systemDataContext;
        }

        public async Task<Dictionary<string, string>> GetContextObjectKeyValuesAsync(object objectInfo, string groupPartitionId, string culture, string timeZone)
        {
            var groupId = (string)objectInfo;
            var group = await _systemDataContext.GetDocumentAsync<Group>(groupId);

            var dic = new Dictionary<string, string>
            {
                { "name", group.Name },
                { "contactFirstName", group.FirstName },
                { "contactLastName", group.LastName },
                { "contactEmail", group.Email },
            };

            return dic;
        }
    }
}
