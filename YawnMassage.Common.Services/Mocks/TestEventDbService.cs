using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Common.Services.Mocks
{
    public class TestEventDbService : EventsDbService
    {
        public TestEventDbService(string groupId, IDocumentDbService documentDbService)
            : base(documentDbService,
                  new DbInfo { DatabaseId = "YawnMassagedb", EventsCollectionId = "YawnMassage_events" },
                  new RequestContext { GroupId = groupId, UserId = "unit-test-user", UserDisplayName = "" })
        {
        }
    }
}
