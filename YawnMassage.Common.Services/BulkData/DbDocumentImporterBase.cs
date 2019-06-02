using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts.BulkData;
using YawnMassage.Common.Domain.Contracts.DataTemplates;
using YawnMassage.Common.Domain.Documents;
using System;
using System.Linq;

namespace YawnMassage.Common.Services.BulkData
{
    /// <summary>
    /// Provides common validation for any type of AuditedDbDocument import.
    /// </summary>
    public abstract class DbDocumentImporterBase<T> : ObjectImporterBase<T>, IBulkDataImporter<T> where T : AuditedDocumentBase, new()
    {
        private readonly RequestContext _requestContext;
        private readonly string _originalGroupContextId;

        public DbDocumentImporterBase(RequestContext requestContext, IBulkDataTagService groupTagService, bool validateGroupTag = true) : base(groupTagService, validateGroupTag)
        {
            _requestContext = requestContext;
            _originalGroupContextId = _requestContext.GroupId;
        }

        protected override void BeforeImport(T obj)
        {
            //Set the request context groupId so that the service call to update/create the imported object
            //will work under the proper group partition.
            _requestContext.GroupId = obj.GroupId;
        }

        protected override void AfterImport(T obj)
        {
            _requestContext.GroupId = _originalGroupContextId;
        }

        protected override void ValidateMappedObject(MappedObject<T> obj)
        {
            //Perform common document object validations.
            var dataRow = obj.DataRowGroup.First();

            string id = obj.Object.Id?.ToLower();
            var idCell = dataRow.GetCellByColumnName("id");

            if (string.IsNullOrEmpty(id))
            {
                idCell.MarkAsRequired();
                dataRow.MarkAsError();
            }
            else if (id == "new")
            {
                obj.Object.Id = null;
            }
            else
            {
                Guid guid;
                var isGuid = Guid.TryParse(id, out guid);
                if (!isGuid)
                {
                    idCell.MarkAsInvalidDataFormat();
                    dataRow.MarkAsError();
                }
            }

            base.ValidateMappedObject(obj);
        }
    }
}
