namespace YawnMassage.Common.Domain.Contexts
{
    /// <summary>
    /// Defines the <see cref="RequestContext" />
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the UserDisplayName
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the GroupId
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the Culture
        /// </summary>
        public string Culture { get; set; }
    }
}
