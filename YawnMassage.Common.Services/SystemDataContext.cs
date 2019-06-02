using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Common.Services
{
    public class SystemDataContext : AuditedDocumentDbService, ISystemDataContext
    {
        public SystemDataContext(IDocumentDbService documentDbService, DbInfo dbInfo, RequestContext requestContext)
            : base(documentDbService, dbInfo, requestContext, useNeutralGroup: true)
        {
            
        }
    }
}
