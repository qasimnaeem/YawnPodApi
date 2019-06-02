namespace YawnMassage.Platform.Domain.Dto.User
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="UserUpdateMessageDto" />
    /// </summary>
    public class UserUpdateMessageDto
    {
        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the GroupRoles
        /// </summary>
        public List<UserGroupRoleDto> GroupRoles { get; set; }
    }
}
