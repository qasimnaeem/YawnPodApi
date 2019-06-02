namespace YawnMassage.Platform.Domain.Dto.Group
{
    using YawnMassage.Common.Domain.Dto;
    using YawnMassage.Platform.Domain.Dto.Shared;

    /// <summary>
    /// Defines the <see cref="GroupDto" />
    /// </summary>
    public class GroupDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

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
        /// Gets or sets the MobileNumber
        /// </summary>
        public MobileNumberDto MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Tag
        /// </summary>
        public string Tag { get; set; }
    }
}
