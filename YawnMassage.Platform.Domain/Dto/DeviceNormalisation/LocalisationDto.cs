namespace YawnMassage.Platform.Domain.Dto.DeviceKeyNormalisation
{
    using YawnMassage.Platform.Domain.Dto.DeviceNormalisation;

    /// <summary>
    /// Defines the <see cref="LocalisationDto" />
    /// </summary>
    public class LocalisationDto : BaseNormalisationDto
    {
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }
    }
}
