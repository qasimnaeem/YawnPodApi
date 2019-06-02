using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;

namespace YawnMassage.Common.Services.Mocks
{
    public class TestGroupDataContextFactory : IGroupDataContextFactory
    {
        private readonly IDocumentDbService _documentDbService;

        public TestGroupDataContextFactory(IDocumentDbService documentDbService)
        {
            _documentDbService = documentDbService;
        }

        public IGroupDataContext CreateGroupDataContext(string groupId)
        {
            return new TestGroupDataContext(groupId, _documentDbService);
        }
    }
}
