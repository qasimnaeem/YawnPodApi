namespace YawnMassage.Platform.Domain.Dto.DataManagement
{
    using System;

    /// <summary>
    /// Defines the <see cref="BulkDataOperationDto" />
    /// </summary>
    public class BulkDataOperationDto
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the OperationTypeCode
        /// </summary>
        public string OperationTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the OperationStatusCode
        /// </summary>
        public string OperationStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the BlobReference
        /// </summary>
        public string BlobReference { get; set; }

        /// <summary>
        /// Gets or sets the LogBlobReference
        /// </summary>
        public string LogBlobReference { get; set; }

        /// <summary>
        /// Gets or sets the GroupId
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsNewDatabaseFile
        /// </summary>
        public bool IsNewDatabaseFile { get; set; }

        /// <summary>
        /// Gets or sets the CreatedOnUtc
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
