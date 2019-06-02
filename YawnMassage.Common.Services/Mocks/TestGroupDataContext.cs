using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Common.Services.Mocks
{
    public class TestGroupDataContext : AuditedDocumentDbService, IGroupDataContext
    {
        public TestGroupDataContext(string groupId, IDocumentDbService documentDbService)
            : base(documentDbService,
                  new DbInfo { DatabaseId = "YawnMassagedb", CollectionId = "YawnMassage_default" },
                  new RequestContext { GroupId = groupId, UserId = "unit-test-user", UserDisplayName = "" },
                  useNeutralGroup: false)
        {
        }
    }
}
