using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Documents;

namespace YawnMassage.Common.Identity.Documents
{
    /// <summary>
    /// Represents a user in the identity system for the <see cref="Stores.DocumentDbUserStore{TUser, TRole}"/> with the role type defaulted to <see cref="DocumentDbIdentityRole"/>
    /// </summary>
    public class DocumentDbIdentityUser : DocumentDbIdentityUser<DocumentDbIdentityRole>
    {
    }

    public class DocumentDbIdentityUser<TRole> : AuditedDocumentBase
    {
        public DocumentDbIdentityUser()
        {
            Roles = new List<TRole>();
            Logins = new List<UserLoginInfo>();
            Claims = new List<Claim>();
        }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty( "email")]
        public string Email { get; set; }

        [JsonProperty("normalizedUserName")]
        public string NormalizedUserName { get; set; }

        [JsonProperty("normalizedEmail")]
        public string NormalizedEmail { get; set; }

        [JsonProperty("isEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; internal set; }

        [JsonProperty("isPhoneNumberConfirmed")]
        public bool IsPhoneNumberConfirmed { get; internal set; }

        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty("securityStamp")]
        public string SecurityStamp { get; set; }

        [JsonProperty("isTwoFactorAuthEnabled")]
        public bool IsTwoFactorAuthEnabled { get; set; }

        [JsonProperty("logins")]
        public IList<UserLoginInfo> Logins { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public IList<TRole> Roles { get; set; }

        [JsonProperty(PropertyName = "claims")]
        public IList<Claim> Claims { get; set; }

        [JsonProperty("lockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonProperty("lockoutEndDate")]
        public DateTimeOffset? LockoutEndDate { get; set; }

        [JsonProperty("accessFailedCount")]
        public int AccessFailedCount { get; set; }

        public override string DocType => DocumentTypes.User;

#if NETSTANDARD2
        [JsonProperty(PropertyName = "authenticatorKey")]
        public string AuthenticatorKey { get; set; }

        [JsonProperty(PropertyName = "recoveryCodes")]
        public IEnumerable<string> RecoveryCodes { get; set; }
#endif
    }
}
