using Newtonsoft.Json;
namespace YawnMassage.Common.Domain.Documents
{
    /// <summary>
    /// Defines the <see cref="EventDocumentBase" />
    /// </summary>
    public abstract class EventDocumentBase
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
        /// Gets or sets the GroupId
        /// </summary>
        [JsonProperty("groupId")]
        public string GroupId { get; set; }

        /// <summary>
        /// Gets the DocType
        /// </summary>
        [JsonProperty("docType")]
        public abstract string DocType { get; }
    }
}
