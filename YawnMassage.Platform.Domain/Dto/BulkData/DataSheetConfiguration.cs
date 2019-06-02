namespace YawnMassage.Platform.Domain.Dto.BulkData
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="DataSheetConfiguration" />
    /// </summary>
    public class DataSheetConfiguration
    {
        /// <summary>
        /// Gets or sets the EntityType
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the SheetIndex
        /// </summary>
        public int SheetIndex { get; set; }

        /// <summary>
        /// Gets or sets the ColumnHeadersRowNo
        /// </summary>
        public int ColumnHeadersRowNo { get; set; }

        /// <summary>
        /// Gets or sets the ColumnMappings
        /// </summary>
        public Dictionary<string, string> ColumnMappings { get; set; }
    }
}
