using Newtonsoft.Json;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Documents;

namespace YawnMassage.Platform.Domain.Documents
{
    

    /// <summary>
    /// Defines the <see cref="BulkDataOperation" />
    /// </summary>
    public class BulkDataOperation : AuditedDocumentBase
    {
        /// <summary>
        /// Gets or sets the RelatedGroupId
        /// </summary>
        [JsonProperty("relatedGroupId")]
        public string RelatedGroupId { get; set; }

        /// <summary>
        /// Gets or sets the RelatedGroupName
        /// </summary>
        [JsonProperty("relatedGroupName")]
        public string RelatedGroupName { get; set; }

        /// <summary>
        /// Gets or sets the RelatedUserId
        /// </summary>
        [JsonProperty("requestedUserId")]
        public string RelatedUserId { get; set; }

        /// <summary>
        /// Gets or sets the BlobReference
        /// </summary>
        [JsonProperty("blobReference")]
        public string BlobReference { get; set; }

        /// <summary>
        /// Gets or sets the LogBlobReference
        /// </summary>
        [JsonProperty("logBlobReference")]
        public string LogBlobReference { get; set; }

        /// <summary>
        /// Gets or sets the FileName
        /// </summary>
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the OperationTypeCode
        /// </summary>
        [JsonProperty("operationTypeCode")]
        public string OperationTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the OperationStatusCode
        /// </summary>
        [JsonProperty("operationStatusCode")]
        public string OperationStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the Remark
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsNewDatabaseFile
        /// </summary>
        [JsonProperty("isNewDatabaseFile")]
        public bool IsNewDatabaseFile { get; set; }

        /// <summary>
        /// Gets the DocType
        /// </summary>
        public override string DocType => DocumentTypes.BulkDataJob;
    }
}
