using System.Collections.Generic;
using YawnMassage.Common.Domain.Contracts.DataTemplates;

namespace YawnMassage.Common.Domain.Contracts.BulkData
{
    /// <summary>
    /// Defines the <see cref="SheetImportResult" />
    /// </summary>
    public class SheetImportResult
    {
        /// <summary>
        /// Gets or sets the SheetName
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// Gets or sets the EntityType
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the ErrorRows
        /// </summary>
        public IEnumerable<ErrorRow> ErrorRows { get; set; }

        /// <summary>
        /// Gets or sets the TotalProcessedRowCount
        /// </summary>
        public int TotalProcessedRowCount { get; set; }

        /// <summary>
        /// Gets or sets the SuccessObjectsCount
        /// </summary>
        public int SuccessObjectsCount { get; set; }

        /// <summary>
        /// Gets or sets the ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
