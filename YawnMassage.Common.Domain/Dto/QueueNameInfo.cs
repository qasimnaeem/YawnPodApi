namespace YawnMassage.Common.Domain.Dto
{
    /// <summary>
    /// Defines the <see cref="QueueNameInfo" />
    /// </summary>
    public class QueueNameInfo
    {
        /// <summary>
        /// Gets or sets the UserUpdateQueueName
        /// </summary>
        public string UserUpdateQueueName { get; set; }

        /// <summary>
        /// Gets or sets the RoleUpdateQueueName
        /// </summary>
        public string RoleUpdateQueueName { get; set; }

        /// <summary>
        /// Gets or sets the PodSnapshotQueueName
        /// </summary>
        public string PodSnapshotQueueName { get; set; }

        /// <summary>
        /// Gets or sets the PodDeletionQueueName
        /// </summary>
        public string PodDeletionQueueName { get; set; }

        /// <summary>
        /// Gets or sets the PodAccessDefinitionQueueName
        /// </summary>
        public string PodAccessDefinitionQueueName { get; set; }

        /// <summary>
        /// Gets or sets the ReportExportQueueName
        /// </summary>
        public string ReportExportQueueName { get; set; }

        /// <summary>
        /// Gets or sets the BulkExportQueueName
        /// </summary>
        public string BulkExportQueueName { get; set; }

        /// <summary>
        /// Gets or sets the BulkImportQueueName
        /// </summary>
        public string BulkImportQueueName { get; set; }

        /// <summary>
        /// Gets or sets the AlertNotifyQueueName
        /// </summary>
        public string AlertNotifyQueueName { get; set; }

        /// <summary>
        /// Gets or sets the DeviceUpdateQueueName
        /// </summary>
        public string DeviceUpdateQueueName { get; set; }
    }
}
