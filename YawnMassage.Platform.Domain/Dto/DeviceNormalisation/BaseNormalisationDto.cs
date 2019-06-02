namespace YawnMassage.Platform.Domain.Dto.DeviceNormalisation
{
    /// <summary>
    /// Defines the <see cref="BaseNormalisationDto" />
    /// </summary>
    public class BaseNormalisationDto
    {
        /// <summary>
        /// Gets or sets the Section
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the Culture
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the Key
        /// </summary>
        public string Key { get; set; }
    }
}
