namespace YawnMassage.Common.Domain.Constants
{
    /// <summary>
    /// Defines the <see cref="AlertNotificationFormats" />
    /// </summary>
    public static class AlertNotificationFormats
    {
        /// <summary>
        /// Defines the DateFormat
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Defines the DateTimeFormat
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm";
    }

    /// <summary>
    /// Defines the <see cref="MasterAlertTemplatePlaceholders" />
    /// </summary>
    public static class MasterAlertTemplatePlaceholders
    {
        /// <summary>
        /// Defines the Subject
        /// </summary>
        public const string Subject = "subject";

        /// <summary>
        /// Defines the Body
        /// </summary>
        public const string Body = "body";
    }

    /// <summary>
    /// Defines the <see cref="AlertObjectContextTypes" />
    /// </summary>
    public static class AlertObjectContextTypes
    {
        /// <summary>
        /// Defines the Pod
        /// </summary>
        public const string Pod = "pod";

        /// <summary>
        /// Defines the Group
        /// </summary>
        public const string Group = "group";

        /// <summary>
        /// Defines the User
        /// </summary>
        public const string User = "user";

        /// <summary>
        /// Defines the System
        /// </summary>
        public const string System = "system";

        /// <summary>
        /// Defines the Report
        /// </summary>
        public const string Report = "report";
    }
}
