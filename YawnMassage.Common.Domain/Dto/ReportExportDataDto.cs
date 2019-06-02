using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Dto
{
    /// <summary>
    /// Defines the <see cref="ReportExportDataDto" />
    /// </summary>
    public class ReportExportDataDto
    {
        /// <summary>
        /// Gets or sets the ColumnNameKeys
        /// </summary>
        public IEnumerable<string> ColumnNameKeys { get; set; }

        /// <summary>
        /// Gets or sets the ContentList
        /// </summary>
        public IEnumerable<IEnumerable<object>> ContentList { get; set; }
    }
}
