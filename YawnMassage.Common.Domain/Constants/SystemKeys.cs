namespace YawnMassage.Common.Domain.Constants
{
    /// <summary>
    /// Defines the <see cref="SystemKeys" />
    /// </summary>
    public static class SystemKeys
    {
        /// <summary>
        /// Defines the <see cref="LookupKeys" />
        /// </summary>
        public static class LookupKeys
        {
            /// <summary>
            /// Defines the LIST_ROLES
            /// </summary>
            public const string LIST_ROLES = "LIST_ROLES";

            /// <summary>
            /// Defines the LIST_ITEM_STATES
            /// </summary>
            public const string LIST_ITEM_STATES = "LIST_ITEM_STATES";

            /// <summary>
            /// Defines the LIST_POD_EVENT_TYPES
            /// </summary>
            public const string LIST_POD_EVENT_TYPES = "LIST_POD_EVENT_TYPES";
        }

        /// <summary>
        /// Defines the <see cref="AlertTemplateKeys" />
        /// </summary>
        public static class AlertTemplateKeys
        {
            /// <summary>
            /// Defines the Master
            /// </summary>
            public const string Master = "MASTER";

            /// <summary>
            /// Defines the UserActivation
            /// </summary>
            public const string UserActivation = "USER_ACTIVATION";

            /// <summary>
            /// Defines the ResetPassword
            /// </summary>
            public const string ResetPassword = "RESET_PASSWORD";

            /// <summary>
            /// Defines the ReportExport
            /// </summary>
            public const string ReportExport = "REPORT_EXPORT";
        }

        /// <summary>
        /// Defines the <see cref="AlertChannelKeys" />
        /// </summary>
        public static class AlertChannelKeys
        {
            /// <summary>
            /// Defines the Email
            /// </summary>
            public const string Email = "EMAIL";

            /// <summary>
            /// Defines the Sms
            /// </summary>
            public const string Sms = "SMS";
        }

        /// <summary>
        /// Defines the <see cref="ConfigurationKeys" />
        /// </summary>
        public static class ConfigurationKeys
        {
            /// <summary>
            /// Defines the SystemHostUrl
            /// </summary>
            public const string SystemHostUrl = "SYSTEM_HOST_BASEURL";

            /// <summary>
            /// Defines the SystemLogoUrl
            /// </summary>
            public const string SystemLogoUrl = "SYSTEM_LOGO_URL";

            /// <summary>
            /// Defines the ActivationEmailAction
            /// </summary>
            public const string ActivationEmailAction = "SYSTEM_ACTIVATIONEMAIL_CONTROLLER_ACTION";

            /// <summary>
            /// Defines the SetPasswordAction
            /// </summary>
            public const string SetPasswordAction = "SYSTEM_SETPASSWORD_CONTROLLER_ACTION";

            /// <summary>
            /// Defines the SendGridApiKey
            /// </summary>
            public const string SendGridApiKey = "EMAIL_SENDGRID_API_KEY";

            /// <summary>
            /// Defines the DefaultGroupRole
            /// </summary>
            public const string DefaultGroupRole = "DEFAULT_GROUP_ROLE";

            /// <summary>
            /// Defines the StoragePrimaryConnectionString
            /// </summary>
            public const string StoragePrimaryConnectionString = "STORAGE_PRIMARY_CONNECTION_STRING";

            /// <summary>
            /// Defines the ServiceBusConnectionString
            /// </summary>
            public const string ServiceBusConnectionString = "SERVICEBUS_CONNECTIONSTRING";

            /// <summary>
            /// Defines the ServiceBusPodConfigQueueName
            /// </summary>
            public const string ServiceBusPodConfigQueueName = "SERVICEBUS_PODCONFIG_QUEUE";

            /// <summary>
            /// Defines the ServiceBusPodDeleteQueueName
            /// </summary>
            public const string ServiceBusPodDeleteQueueName = "SERVICEBUS_PODDELETE_QUEUE";

            /// <summary>
            /// Defines the TableStorageConnectionString
            /// </summary>
            public const string TableStorageConnectionString = "TABLE_STORAGE_CONNECTION_STRING";

            /// <summary>
            /// Defines the BulkDataSheetConfiguration
            /// </summary>
            public const string BulkDataSheetConfiguration = "BULKDATA_SHEET_CONFIGURATION";

            /// <summary>
            /// Defines the BulkDataTemplateFileName
            /// </summary>
            public const string BulkDataTemplateFileName = "BULKDATA_TEMPLATE_FILENAME";

            /// <summary>
            /// Defines the ReportExportExpiryHours
            /// </summary>
            public const string ReportExportExpiryHours = "REPORT_EXPORT_EXPIRY_HOURS";

            /// <summary>
            /// Defines the ManifestDownloadExpiryHours
            /// </summary>
            public const string ManifestDownloadExpiryHours = "MANIFEST_DOWNLOAD_EXPIRY_HOURS";
        }

        /// <summary>
        /// Defines the <see cref="BulkData" />
        /// </summary>
        public static class BulkData
        {
            /// <summary>
            /// Defines the PermissionKey
            /// </summary>
            public const string PermissionKey = "BULKDATA_SEARCH";
        }

        /// <summary>
        /// Defines the <see cref="LocalisationKeys" />
        /// </summary>
        public static class LocalisationKeys
        {
            /// <summary>
            /// Defines the ProductName
            /// </summary>
            public const string ProductName = "TEXT_PRODUCT_NAME";

            /// <summary>
            /// Defines the CompanyName
            /// </summary>
            public const string CompanyName = "TEXT_COMPANY_NAME";
        }
    }
}
