using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Contracts.BulkData;

namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IDataExportService" />
    /// </summary>
    public interface IDataExportService
    {
        /// <summary>
        /// The ExportAsync
        /// </summary>
        /// <param name="bulkDataOperationId">The bulkDataOperationId<see cref="string"/></param>
        /// <param name="tagServices">The tagServices<see cref="IEnumerable{IBulkDataTagService}"/></param>
        /// <param name="exporters">The exporters<see cref="IEnumerable{KeyValuePair{string, IBulkDataExporter}}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ExportAsync(string bulkDataOperationId, IEnumerable<IBulkDataTagService> tagServices, IEnumerable<KeyValuePair<string, IBulkDataExporter>> exporters);
    }
}
