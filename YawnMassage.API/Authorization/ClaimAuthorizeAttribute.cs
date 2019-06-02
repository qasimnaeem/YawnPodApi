using Microsoft.AspNetCore.Authorization;

namespace YawnMassage.Api.Authorization
{
    public class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        public const string POLICY_PREFIX = "REQUIRECLAIM-";

        public ClaimAuthorizeAttribute(string claim)
        {
            Claim = claim;
        }

        public string Claim
        {
            get
            {
                return Policy.Substring(POLICY_PREFIX.Length);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
