namespace YawnMassage.Platform.Domain.Dto.User
{
    using System;
    using System.Collections.Generic;
    using YawnMassage.Common.Domain.Dto;
    using YawnMassage.Platform.Domain.Dto.Shared;

    /// <summary>
    /// Defines the <see cref="UserDto" />
    /// </summary>
    public class UserDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the FullName
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the Culture
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the TimeZone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the MobileNumber
        /// </summary>
        public MobileNumberDto MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }



        /// <summary>
        /// Gets or sets a value indicating whether IsEmailConfirmed
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsActivationEmailSent
        /// </summary>
        public bool IsActivationEmailSent { get; set; }

        /// <summary>
        /// Gets or sets the AlternateId
        /// </summary>
        public string AlternateId { get; set; }

        /// <summary>
        /// Gets or sets the PIN
        /// </summary>
        public string PIN { get; set; }

        /// <summary>
        /// Gets or sets the Tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the AccessExpiryDate
        /// </summary>
        public DateTime? AccessExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the GroupRoles
        /// </summary>
        public List<UserGroupRoleDto> GroupRoles { get; set; }

        /// <summary>
        /// Get or set the user location
        /// </summary>
        public UserLocationDto UserLocation { get; set; }

        /// <summary>
        /// Get or set the user image blob id
        /// </summary>
        public string ImageBlobId { get; set; }
        
        /// <summary>
        /// Get or set the user password
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Get or set the Purpose for yawnMassage
        /// </summary>
        public List<string> Purposes { get; set; }

        public UserDto()
        {
            ImageBlobId = string.Empty;
            Purposes=new List<string>();
        }
    }

    /// <summary>
    /// Defines the <see cref="UserGroupRoleDto" />
    /// </summary>
    public class UserGroupRoleDto
    {
        /// <summary>
        /// Gets or sets the GroupId
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the Role
        /// </summary>
        public string Role { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="UserLocationDto" />
    /// </summary>
    public class UserLocationDto
    {
        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or set the city
        /// </summary>
        public string City { get; set; }
    }
}
