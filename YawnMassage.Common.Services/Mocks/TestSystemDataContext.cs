using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Services;

namespace YawnMassage.Common.Services.Mocks
{
    public class TestSystemDataContext : AuditedDocumentDbService, ISystemDataContext
    {
        public TestSystemDataContext(IDocumentDbService documentDbService)
            : base(documentDbService,
                  new DbInfo { DatabaseId = "YawnMassagedb", CollectionId = "YawnMassage_default" },
                  new RequestContext { UserId = "unit-test-user", UserDisplayName = "" },
                  useNeutralGroup: true)
        {
        }
    }
}
