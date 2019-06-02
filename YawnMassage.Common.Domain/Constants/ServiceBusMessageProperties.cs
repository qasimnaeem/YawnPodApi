namespace YawnMassage.Common.Domain.Constants
{
    /// <summary>
    /// Defines the <see cref="ServiceBusMessageProperties" />
    /// </summary>
    public static class ServiceBusMessageProperties
    {
        /// <summary>
        /// Defines the MessageType
        /// </summary>
        public const string MessageType = "MessageType";

        /// <summary>
        /// Defines the GroupId
        /// </summary>
        public const string GroupId = "GroupId";

        /// <summary>
        /// Defines the UserId
        /// </summary>
        public const string UserId = "UserId";

        /// <summary>
        /// Defines the UserDisplayName
        /// </summary>
        public const string UserDisplayName = "UserDisplayName";

        /// <summary>
        /// Defines the Culture
        /// </summary>
        public const string Culture = "Culture";

        /// <summary>
        /// Defines the MasterReportType
        /// </summary>
        public const string MasterReportType = "MasterReportType";

        /// <summary>
        /// Defines the ReportType
        /// </summary>
        public const string ReportType = "ReportType";
    }

    /// <summary>
    /// Defines the <see cref="ServiceBusMessageTypes" />
    /// </summary>
    public static class ServiceBusMessageTypes
    {
        /// <summary>
        /// Defines the PodUpdate
        /// </summary>
        public const string PodUpdate = "POD_UPDATE";

        /// <summary>
        /// Defines the PodDelete
        /// </summary>
        public const string PodDelete = "POD_DELETE";

        /// <summary>
        /// Defines the ReportExport
        /// </summary>
        public const string ReportExport = "REPORT_EXPORT";
    }
}
