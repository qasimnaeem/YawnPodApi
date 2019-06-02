namespace YawnMassage.Platform.Domain.Dto.ConfigurationSetting
{
    using YawnMassage.Platform.Domain.Dto.Configuration;

    /// <summary>
    /// Defines the <see cref="ConfigurationSettingDto" />
    /// </summary>
    public class ConfigurationSettingDto : ConfigurationBaseDto
    {
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }
    }
}
