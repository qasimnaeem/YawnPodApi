namespace YawnMassage.Platform.Domain.Documents
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using YawnMassage.Common.Identity.Documents;
    using YawnMassage.Platform.Domain.Documents.Shared;

    /// <summary>
    /// Defines the <see cref="User" />
    /// </summary>
    public class User : DocumentDbIdentityUser
    {
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
        /// Gets or sets the NormalizedFullName
        /// </summary>
        [JsonProperty("normalizedFullName")]
        public string NormalizedFullName { get; set; }

        /// <summary>
        /// Gets or sets the Culture
        /// </summary>
        [JsonProperty("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the TimeZone
        /// </summary>
        [JsonProperty("timezone")]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the MobileNumber
        /// </summary>
        [JsonProperty("mobilenumber")]
        public MobileNumber MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the AlternateId
        /// </summary>
        [JsonProperty("alternateId")]
        public string AlternateId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsActivationEmailSent
        /// </summary>
        [JsonProperty("isActivationEmailSent")]
        public bool IsActivationEmailSent { get; set; }

        /// <summary>
        /// Gets or sets the PIN
        /// </summary>
        [JsonProperty("pin")]
        public string PIN { get; set; }

        /// <summary>
        /// Gets or sets the AccessExpiryDate
        /// </summary>
        [JsonProperty("expirydate")]
        public DateTime? AccessExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the GroupRoles
        /// </summary>
        [JsonProperty("groupRoles")]
        public List<UserGroupRole> GroupRoles { get; set; }

        /// <summary>
        /// Gets or sets the userLocation
        /// </summary>
        [JsonProperty("userlocation")]
        public UserLocation UserLocation { get; set; }
        /// <summary>
        /// Gets or sets the Tag
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }
        
        /// <summary>
        /// Gets or sets the user profile image blobid
        /// </summary>
        [JsonProperty("imageblobid")]
        public string ImageBlobId { get; set; }

        /// <summary>
        /// Get or set the Purpose for yawnMassage
        /// </summary>
        [JsonProperty("purposes")]
        public List<string> Purposes { get; set; }

        public User()
        {
            ImageBlobId = string.Empty;
            Purposes=new List<string>();
        }
    }

    /// <summary>
    /// Defines the <see cref="UserGroupRole" />
    /// </summary>
    public class UserGroupRole
    {
        /// <summary>
        /// Gets or sets the Group
        /// </summary>
        [JsonProperty("group")]
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the Role
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="UserLocation" />
    /// </summary>
    public class UserLocation
    {
        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the State
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Get or set the city
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
    }
}
