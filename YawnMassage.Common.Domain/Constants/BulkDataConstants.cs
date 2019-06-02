namespace YawnMassage.Common.Domain.Constants
{
    /// <summary>
    /// Defines the <see cref="BulkDataOperations" />
    /// </summary>
    public class BulkDataOperations
    {
        /// <summary>
        /// Defines the ExportOperation
        /// </summary>
        public const string ExportOperation = "EXPORT";

        /// <summary>
        /// Defines the ImportOperation
        /// </summary>
        public const string ImportOperation = "IMPORT";
    }

    /// <summary>
    /// Defines the <see cref="BulkDataFileDetails" />
    /// </summary>
    public class BulkDataFileDetails
    {
        /// <summary>
        /// Defines the ExportFileNamePrefix
        /// </summary>
        public const string ExportFileNamePrefix = "yawnpod_db_bulk_data_";

        /// <summary>
        /// Defines the ExportFileExtension
        /// </summary>
        public const string ExportFileExtension = "xlsx";

        /// <summary>
        /// Defines the FileGuideSheetName
        /// </summary>
        public const string FileGuideSheetName = "0 - File Guide";
    }
}
