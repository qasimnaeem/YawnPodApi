namespace YawnMassage.Platform.Domain.Dto.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="UILookupDto" />
    /// </summary>
    public class UILookupDto
    {
        /// <summary>
        /// Gets or sets the GroupId
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the Culture
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the Section
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the Priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the Items
        /// </summary>
        public IEnumerable<LookupItemDto> Items { get; set; }
    }
}
