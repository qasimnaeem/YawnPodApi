using Newtonsoft.Json;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Documents;
using YawnMassage.Platform.Domain.Documents.Shared;
namespace YawnMassage.Platform.Domain.Documents
{
    /// <summary>
    /// Defines the <see cref="Group" />
    /// </summary>
    public class Group : AuditedDocumentBase
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the FullName
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the MobileNumber
        /// </summary>
        [JsonProperty("mobilenumber")]
        public MobileNumber MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Tag
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Gets the DocType
        /// </summary>
        public override string DocType => DocumentTypes.Group;
    }
}
