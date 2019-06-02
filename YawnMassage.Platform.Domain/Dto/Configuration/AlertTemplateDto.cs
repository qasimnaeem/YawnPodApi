using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Platform.Domain.Dto.Configuration
{
    /// <summary>
    /// Defines the <see cref="AlertTemplateDto" />
    /// </summary>
    public class AlertTemplateDto : BaseDto
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
        /// Gets or sets the Priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the Channel
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the SenderId
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Gets or sets the Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the Remark
        /// </summary>
        public string Remark { get; set; }
    }
}
