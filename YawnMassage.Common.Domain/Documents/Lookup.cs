using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using YawnMassage.Common.Domain.Constants;
namespace YawnMassage.Common.Domain.Documents
{
    /// <summary>
    /// Defines the <see cref="Lookup" />
    /// </summary>
    public class Lookup : AuditedDocumentBase
    {
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
        /// Gets or sets the Key
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IncludeInPod
        /// </summary>
        [JsonProperty("includeInPod")]
        public bool IncludeInPod { get; set; }

        /// <summary>
        /// Gets or sets the Items
        /// </summary>
        [JsonProperty("items")]
        public List<LookupItem> Items { get; set; }

        /// <summary>
        /// Gets the DocType
        /// </summary>
        public override string DocType => DocumentTypes.Lookup;

        /// <summary>
        /// The GetText
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GetText(string value)
        {
            var text = this.Items?.FirstOrDefault(i => i.Value == value)?.Text ?? value;
            return text;
        }
    }

    /// <summary>
    /// Defines the <see cref="LookupItem" />
    /// </summary>
    public class LookupItem
    {
        /// <summary>
        /// Gets or sets the Text
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

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
        /// Gets or sets the ChildLookupKey
        /// </summary>
        [JsonProperty("childLookupKey")]
        public string ChildLookupKey { get; set; }

        /// <summary>
        /// Gets or sets the SortOrder
        /// </summary>
        [JsonProperty("sortOrder")]
        public int? SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the flag for country (in case of country lookup)
        /// </summary>
        [JsonProperty("flag")]
        public string Flag { get; set; }
    }
}
