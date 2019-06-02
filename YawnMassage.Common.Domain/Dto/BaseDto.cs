using System;
namespace YawnMassage.Common.Domain.Dto
{
    /// <summary>
    /// Defines the <see cref="BaseDto" />
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the GroupId
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedById
        /// </summary>
        public string UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedByName
        /// </summary>
        public string UpdatedByName { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedOnUtc
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the ETag
        /// </summary>
        public string ETag { get; set; }
    }
}
