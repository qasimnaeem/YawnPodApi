using System;
namespace YawnMassage.Common.Domain.Dto
{
    /// <summary>
    /// Defines the <see cref="DocumentUpdateResultDto" />
    /// </summary>
    public class DocumentUpdateResultDto
    {
        /// <summary>
        /// Get or set if operation successful
        /// </summary>
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// Get or set if operation fail
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the ETag
        /// </summary>
        public string ETag { get; set; }

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
    }
}
