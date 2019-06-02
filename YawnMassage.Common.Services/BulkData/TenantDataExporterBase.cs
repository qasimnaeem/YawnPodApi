using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.BulkData;
using YawnMassage.Common.Domain.Contracts.DataTemplates;
using YawnMassage.Common.Domain.Contracts.Factories;
using YawnMassage.Common.Domain.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services.BulkData.Exporters
{
    /// <summary>
    /// Provides common functionality to export tenant-specific data objects.
    /// All exported which are exporting tenant-data can inherit from this class.
    /// </summary>
    public abstract class TenantDataExporterBase<T> : IBulkDataExporter<T> where T : AuditedDocumentBase, new()
    {
        private readonly ISystemDataContext _systemDataContext;
        private readonly IGroupDataContextFactory _groupDataContextFactory;
        private readonly Func<IQueryable<T>, IQueryable<T>> _query;

        public TenantDataExporterBase(Func<IQueryable<T>, IQueryable<T>> query,
            ISystemDataContext systemDataContext, IGroupDataContextFactory groupDataContextFactory)
        {
            _systemDataContext = systemDataContext;
            _groupDataContextFactory = groupDataContextFactory;
            _query = query;
        }

        protected abstract ITemplateDataAdapter<T> GetDataAdapter(ITemplateSheetDataStore datastore, Dictionary<string, string> columnMappings);
        
        public virtual async Task ExportAsync(IEnumerable<string> groupIds, ITemplateSheetDataStore datastore, Dictionary<string, string> columnMappings)
        {
            var objects = await CollectPermittedObjectsAsync(groupIds);
            var dataAdapter = GetDataAdapter(datastore, columnMappings);
            dataAdapter.WriteObjects(objects);
        }

        /// <summary>
        /// Look through all permitted tenant partitions and collect all objects to be exported.
        /// </summary>
        protected async Task<IEnumerable<T>> CollectPermittedObjectsAsync(IEnumerable<string> groupIds)
        {
            var allResults = new List<T>();

            //Iterate group data stores and collect relevant data.
            foreach (var groupId in groupIds)
            {
                var dataContext = _groupDataContextFactory.CreateGroupDataContext(groupId);
                var results = await dataContext.GetDocumentsAsync(_query);
                allResults.AddRange(results);
            }

            return allResults;
        }
    }
}
