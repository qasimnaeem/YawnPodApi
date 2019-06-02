using Newtonsoft.Json;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Documents;

namespace YawnMassage.Platform.Domain.Documents
{
    /// <summary>
    /// Defines the <see cref="AlertTemplate" />
    /// </summary>
    public class AlertTemplate : AuditedDocumentBase
    {
        /// <summary>
        /// Gets or sets the Key
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the Culture
        /// </summary>
        [JsonProperty("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the Section
        /// </summary>
        [JsonProperty("section")]
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the Priority
        /// </summary>
        [JsonProperty("priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the Channel
        /// </summary>
        [JsonProperty("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the Subject
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the SenderId
        /// </summary>
        [JsonProperty("senderId")]
        public string SenderId { get; set; }

        /// <summary>
        /// Gets or sets the Content
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the Remark
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// Gets the DocType
        /// </summary>
        public override string DocType => DocumentTypes.AlertTemplate;
    }
}
