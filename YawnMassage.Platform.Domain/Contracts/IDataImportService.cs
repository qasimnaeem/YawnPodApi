using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Contracts.BulkData;

namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IDataImportService" />
    /// </summary>
    public interface IDataImportService
    {
        /// <summary>
        /// The ImportAsync
        /// </summary>
        /// <param name="bulkDataOperationId">The bulkDataOperationId<see cref="string"/></param>
        /// <param name="tagServices">The tagServices<see cref="IEnumerable{IBulkDataTagService}"/></param>
        /// <param name="importers">The importers<see cref="IEnumerable{KeyValuePair{string, IBulkDataImporter}}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ImportAsync(string bulkDataOperationId, IEnumerable<IBulkDataTagService> tagServices, IEnumerable<KeyValuePair<string, IBulkDataImporter>> importers);
    }
}
