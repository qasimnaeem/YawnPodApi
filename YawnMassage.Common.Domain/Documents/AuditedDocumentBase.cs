using Newtonsoft.Json;
using System;
namespace YawnMassage.Common.Domain.Documents
{
    /// <summary>
    /// Defines the <see cref="AuditedDocumentBase" />
    /// </summary>
    public abstract class AuditedDocumentBase
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the PartitionKey
        /// </summary>
        [JsonProperty("partitionKey")]
        public virtual string PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the ETag
        /// </summary>
        [JsonProperty("_etag")]
        public string ETag { get; set; }

        /// <summary>
        /// Gets or sets the GroupId
        /// </summary>
        [JsonProperty("groupId")]
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedOnUtc
        /// </summary>
        [JsonProperty("createdOnUtc")]
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the CreatedById
        /// </summary>
        [JsonProperty("createdById")]
        public string CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the CreatedByName
        /// </summary>
        [JsonProperty("createdByName")]
        public string CreatedByName { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedOnUtc
        /// </summary>
        [JsonProperty("updatedOnUtc")]
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedById
        /// </summary>
        [JsonProperty("updatedById")]
        public string UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedByName
        /// </summary>
        [JsonProperty("updatedByName")]
        public string UpdatedByName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsDeleted
        /// </summary>
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets the DocType
        /// </summary>
        [JsonProperty("docType")]
        public abstract string DocType { get; }
    }
}
