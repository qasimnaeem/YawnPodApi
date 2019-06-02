using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Platform.Domain.Dto.Configuration
{
    /// <summary>
    /// Defines the <see cref="ConfigurationBaseDto" />
    /// </summary>
    public class ConfigurationBaseDto : BaseDto
    {
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
        /// Gets or sets the Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IncludeInPod
        /// </summary>
        public bool IncludeInPod { get; set; }

        /// <summary>
        /// Gets or sets the Priority
        /// </summary>
        public int Priority { get; set; }
    }
}
