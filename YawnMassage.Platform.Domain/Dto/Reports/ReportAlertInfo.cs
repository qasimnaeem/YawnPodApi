namespace YawnMassage.Platform.Domain.Dto.Reports
{
    using System;

    /// <summary>
    /// Defines the <see cref="ReportAlertInfo" />
    /// </summary>
    public class ReportAlertInfo
    {
        /// <summary>
        /// Gets or sets the ReportName
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the GeneratedOnUtc
        /// </summary>
        public DateTime GeneratedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the ExpiryHours
        /// </summary>
        public int ExpiryHours { get; set; }

        /// <summary>
        /// Gets or sets the DownloadUrl
        /// </summary>
        public string DownloadUrl { get; set; }
    }
}
