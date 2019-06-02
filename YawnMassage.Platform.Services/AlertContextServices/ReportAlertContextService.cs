using YawnMassage.Platform.Domain.Dto.Reports;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.AlertContextServices
{
    public class ReportAlertContextService : IAlertContextObjectService
    {
        public Task<Dictionary<string, string>> GetContextObjectKeyValuesAsync(object objectInfo, string groupId, string culture, string timeZone)
        {
            var report = (objectInfo as JObject).ToObject<ReportAlertInfo>();

            var targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var dateTimeInTargetTimeZone = TimeZoneInfo.ConvertTimeFromUtc(report.GeneratedOnUtc, targetTimeZone);

            var dic = new Dictionary<string, string>
            {
                { "name", report.ReportName },
                { "downloadUrl", report.DownloadUrl },
                { "generatedOn", dateTimeInTargetTimeZone.ToString(AlertNotificationFormats.DateTimeFormat) },
                { "expiryHours", report.ExpiryHours.ToString() }
            };

            return Task.FromResult(dic);
        }
    }
}
