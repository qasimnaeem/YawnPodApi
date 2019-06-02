using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Common.Services
{
    public class GroupDataContext : AuditedDocumentDbService, IGroupDataContext
    {
        public GroupDataContext(IDocumentDbService documentDbService, DbInfo dbInfo, RequestContext requestContext)
            : base(documentDbService, dbInfo, requestContext, useNeutralGroup: false)
        {
        }
    }
}
