using System.Collections.Generic;
using YawnMassage.Common.Domain.Contracts.DataTemplates;

namespace YawnMassage.Common.Domain.Contracts.BulkData
{
    /// <summary>
    /// Defines the <see cref="ImportResult" />
    /// </summary>
    public class ImportResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportResult"/> class.
        /// </summary>
        public ImportResult()
        {
            ErrorRows = new List<ErrorRow>();
        }

        /// <summary>
        /// Gets or sets the TotalProcessedRowCount
        /// </summary>
        public int TotalProcessedRowCount { get; set; }

        /// <summary>
        /// Gets or sets the SuccessObjectsCount
        /// </summary>
        public int SuccessObjectsCount { get; set; }

        /// <summary>
        /// Gets or sets the ErrorRows
        /// </summary>
        public IEnumerable<ErrorRow> ErrorRows { get; set; }
    }
}
