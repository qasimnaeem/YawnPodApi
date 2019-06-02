using System.Collections.Generic;
using System.Threading.Tasks;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IReportExportService" />
    /// </summary>
    public interface IReportExportService
    {
        /// <summary>
        /// The ExportAndNotifyAsync
        /// </summary>
        /// <param name="masterReportType">The masterReportType<see cref="string"/></param>
        /// <param name="reportType">The reportType<see cref="string"/></param>
        /// <param name="columnLocalisationKeys">The columnLocalisationKeys<see cref="IEnumerable{string}"/></param>
        /// <param name="contentList">The contentList<see cref="IEnumerable{IEnumerable{object}}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ExportAndNotifyAsync(string masterReportType, string reportType, IEnumerable<string> columnLocalisationKeys, IEnumerable<IEnumerable<object>> contentList);
    }
}
