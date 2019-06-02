using Newtonsoft.Json;
using System.Collections.Generic;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Documents;

namespace YawnMassage.Platform.Domain.Documents
{
    /// <summary>
    /// Defines the <see cref="PermissionConfig" />
    /// </summary>
    public class PermissionConfig : AuditedDocumentBase
    {
        /// <summary>
        /// Gets or sets the Role
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the Permissions
        /// </summary>
        [JsonProperty("permissions")]
        public List<string> Permissions { get; set; }

        /// <summary>
        /// Gets the DocType
        /// </summary>
        public override string DocType => DocumentTypes.PermissionConfig;
    }
}
