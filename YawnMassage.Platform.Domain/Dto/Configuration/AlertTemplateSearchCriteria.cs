namespace YawnMassage.Platform.Domain.Dto.Configuration
{
    /// <summary>
    /// Defines the <see cref="AlertTemplateSearchCriteria" />
    /// </summary>
    public class AlertTemplateSearchCriteria
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
        /// Gets or sets the Channel
        /// </summary>
        public string Channel { get; set; }
    }
}
