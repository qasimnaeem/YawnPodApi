using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Common.Services.Factories
{
    public class GroupDataContextFactory : IGroupDataContextFactory
    {
        private readonly IDocumentDbService _documentDbService;
        private readonly DbInfo _dbInfo;
        private readonly RequestContext _requestContext;

        public GroupDataContextFactory(IDocumentDbService documentDbService, DbInfo dbInfo, RequestContext requestContext)
        {
            _documentDbService = documentDbService;
            _dbInfo = dbInfo;
            _requestContext = requestContext;
        }

        public IGroupDataContext CreateGroupDataContext(string groupId)
        {
            var dataContext = new GroupDataContext(_documentDbService, _dbInfo, new RequestContext
            {
                GroupId = groupId,
                Culture = _requestContext.Culture,
                UserId = _requestContext.UserId,
                UserDisplayName = _requestContext.UserDisplayName
            });

            return dataContext;
        }
    }
}
