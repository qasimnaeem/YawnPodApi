namespace YawnMassage.Platform.Domain.Dto.Configuration
{
    using System.Collections.Generic;
    using YawnMassage.Common.Domain.Dto;

    /// <summary>
    /// Defines the <see cref="PermissionConfigDto" />
    /// </summary>
    public class PermissionConfigDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the Priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the Permissions
        /// </summary>
        public List<string> Permissions { get; set; }
    }
}
