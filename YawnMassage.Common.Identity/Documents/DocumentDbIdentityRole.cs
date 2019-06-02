using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Documents;

namespace YawnMassage.Common.Identity.Documents
{
    public class DocumentDbIdentityRole : AuditedDocumentBase
    {
        public DocumentDbIdentityRole()
        {
            this.Claims = new List<Claim>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "normalizedName")]
        public string NormalizedName { get; set; }

        [JsonProperty(PropertyName = "claims")]
        public IList<Claim> Claims { get; set; }

        public override string DocType => DocumentTypes.IdentityRole;
    }
}
