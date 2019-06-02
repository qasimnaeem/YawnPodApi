namespace YawnMassage.Platform.Domain.Dto.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="RoleUpdateMessageDto" />
    /// </summary>
    public class RoleUpdateMessageDto
    {
        /// <summary>
        /// Gets or sets the GroupId
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the Roles
        /// </summary>
        public List<string> Roles { get; set; }
    }
}
