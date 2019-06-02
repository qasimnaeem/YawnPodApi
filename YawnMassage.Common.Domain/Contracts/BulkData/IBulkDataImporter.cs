using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Contracts.DataTemplates;

namespace YawnMassage.Common.Domain.Contracts.BulkData
{
    //Marker interface with generic type constraint.
    /// <summary>
    /// Defines the <see cref="IBulkDataImporter{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBulkDataImporter<T> : IBulkDataImporter
    {
    }

    /// <summary>
    /// Defines the <see cref="IBulkDataImporter" />
    /// </summary>
    public interface IBulkDataImporter
    {
        /// <summary>
        /// The ImportAsync
        /// </summary>
        /// <param name="includedGroupIds">The includedGroupIds<see cref="IEnumerable{string}"/></param>
        /// <param name="datastore">The datastore<see cref="ITemplateSheetDataStore"/></param>
        /// <param name="columnMappings">The columnMappings<see cref="Dictionary{string, string}"/></param>
        /// <returns>The <see cref="Task{ImportResult}"/></returns>
        Task<ImportResult> ImportAsync(IEnumerable<string> includedGroupIds,
            ITemplateSheetDataStore datastore, Dictionary<string, string> columnMappings);
    }
}
