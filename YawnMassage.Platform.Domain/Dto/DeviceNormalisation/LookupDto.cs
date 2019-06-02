namespace YawnMassage.Platform.Domain.Dto.DeviceNormalisation
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="LookupDto" />
    /// </summary>
    public class LookupDto : BaseNormalisationDto
    {
        /// <summary>
        /// Gets or sets the Values
        /// </summary>
        public IEnumerable<LookupValueDto> Values { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="LookupValueDto" />
    /// </summary>
    public class LookupValueDto
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
        /// Gets or sets the ChildLookupKey
        /// </summary>
        public string ChildLookupKey { get; set; }

        /// <summary>
        /// Gets or sets the SortOrder
        /// </summary>
        public int? SortOrder { get; set; }
    }
}
