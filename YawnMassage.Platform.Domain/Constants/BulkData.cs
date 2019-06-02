namespace YawnMassage.Platform.Domain.Constants
{
    /// <summary>
    /// Defines the <see cref="BulkDataOperationType" />
    /// </summary>
    public static class BulkDataOperationType
    {
        /// <summary>
        /// Defines the Import
        /// </summary>
        public const string Import = "IMPORT";

        /// <summary>
        /// Defines the Export
        /// </summary>
        public const string Export = "EXPORT";
    }

    /// <summary>
    /// Defines the <see cref="BulkDataOperationStatus" />
    /// </summary>
    public static class BulkDataOperationStatus
    {
        /// <summary>
        /// Defines the Queued
        /// </summary>
        public const string Queued = "QUEUED";

        /// <summary>
        /// Defines the Running
        /// </summary>
        public const string Running = "RUNNING";

        /// <summary>
        /// Defines the Complete
        /// </summary>
        public const string Complete = "COMPLETE";

        /// <summary>
        /// Defines the CompleteWithErrors
        /// </summary>
        public const string CompleteWithErrors = "COMPLETE_WITH_ERRORS";

        /// <summary>
        /// Defines the Failed
        /// </summary>
        public const string Failed = "FAILED";
    }
}
