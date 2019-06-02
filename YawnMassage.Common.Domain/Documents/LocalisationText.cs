using Newtonsoft.Json;
using YawnMassage.Common.Domain.Constants;
namespace YawnMassage.Common.Domain.Documents
{
    /// <summary>
    /// Defines the <see cref="LocalisationText" />
    /// </summary>
    public class LocalisationText : AuditedDocumentBase
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
        /// Gets or sets a value indicating whether IncludeInPod
        /// </summary>
        [JsonProperty("includeInPod")]
        public bool IncludeInPod { get; set; }

        /// <summary>
        /// Gets or sets the Priority
        /// </summary>
        [JsonProperty("priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the Remark
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// Gets the DocType
        /// </summary>
        public override string DocType => DocumentTypes.LocalisationText;
    }
}
