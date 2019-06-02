using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Domain.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.AlertContextServices
{
    public class UserAlertContextService : IAlertContextObjectService
    {
        private readonly ISystemDataContext _systemDataContext;

        public UserAlertContextService(ISystemDataContext systemDataContext)
        {
            _systemDataContext = systemDataContext;
        }

        public async Task<Dictionary<string, string>> GetContextObjectKeyValuesAsync(object objectInfo, string groupId, string culture, string timeZone)
        {
            var userId = (string)objectInfo;
            var user = await _systemDataContext.GetDocumentAsync<User>(userId);

            var dic = new Dictionary<string, string>
            {
                { "firstName", user.FirstName },
                { "lastName", user.LastName },
                { "email", user.Email }
            };

            return dic;
        }
    }
}
