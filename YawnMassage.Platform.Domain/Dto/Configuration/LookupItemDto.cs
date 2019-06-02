namespace YawnMassage.Platform.Domain.Dto.Configuration
{
    /// <summary>
    /// Defines the <see cref="LookupItemDto" />
    /// </summary>
    public class LookupItemDto
    {
        /// <summary>
        /// Gets or sets the Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets the ChildLookupKey
        /// </summary>
        public string ChildLookupKey { get; set; }

        /// <summary>
        /// Gets or sets the SortOrder
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the SortOrder
        /// </summary>
        public string Flag { get; set; }
    }
}
