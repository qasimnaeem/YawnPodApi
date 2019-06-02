using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Dto.Alerts
{
    /// <summary>
    /// Defines the <see cref="AlertNotificationRequestDto" />
    /// </summary>
    public class AlertNotificationRequestDto
    {
        /// <summary>
        /// Gets or sets the TemplateKey
        /// </summary>
        public string TemplateKey { get; set; }

        /// <summary>
        /// Gets or sets the ChannelKey
        /// </summary>
        public string ChannelKey { get; set; }

        /// <summary>
        /// Gets or sets the ContextObjectInfos
        /// Dictionary with object type keys and object identifiers as values.
        /// </summary>
        public Dictionary<string, object> ContextObjectInfos { get; set; }

        /// <summary>
        /// Gets or sets the CustomParamReplacements
        /// Dictionary with template placeholder keys and replacements as values.
        /// </summary>
        public Dictionary<string, string> CustomParamReplacements { get; set; }

        /// <summary>
        /// Gets or sets the RecipientUserId
        /// </summary>
        public string RecipientUserId { get; set; }
    }
}
