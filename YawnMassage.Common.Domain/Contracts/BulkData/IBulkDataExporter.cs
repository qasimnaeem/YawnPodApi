using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Contracts.DataTemplates;
namespace YawnMassage.Common.Domain.Contracts.BulkData
{   
    //Marker interface with generic type constraint.
    /// <summary>
    /// Defines the <see cref="IBulkDataExporter{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBulkDataExporter<T> : IBulkDataExporter
    {
    }

    /// <summary>
    /// Defines the <see cref="IBulkDataExporter" />
    /// </summary>
    public interface IBulkDataExporter
    {
        /// <summary>
        /// The ExportAsync
        /// </summary>
        /// <param name="groupIds">The groupIds<see cref="IEnumerable{string}"/></param>
        /// <param name="datastore">The datastore<see cref="ITemplateSheetDataStore"/></param>
        /// <param name="columnMappings">The columnMappings<see cref="Dictionary{string, string}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ExportAsync(IEnumerable<string> groupIds, ITemplateSheetDataStore datastore, Dictionary<string, string> columnMappings);
    }
}
