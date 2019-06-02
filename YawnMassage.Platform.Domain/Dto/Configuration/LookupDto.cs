namespace YawnMassage.Platform.Domain.Dto.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="LookupDto" />
    /// </summary>
    public class LookupDto : ConfigurationBaseDto
    {
        /// <summary>
        /// Gets or sets the Items
        /// </summary>
        public List<LookupItemDto> Items { get; set; }

        /// <summary>
        /// Gets or sets the ItemValueList
        /// </summary>
        public IEnumerable<string> ItemValueList { get; set; }
    }
}
