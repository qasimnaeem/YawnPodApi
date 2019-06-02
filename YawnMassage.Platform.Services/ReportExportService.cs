using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Reports;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Contracts.DataTemplates;
using YawnMassage.Common.Domain.Dto.Blob;
using YawnMassage.Common.Domain.Enum;
using YawnMassage.Common.Services.DataTemplates;
using YawnMassage.Common.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class ReportExportService : IReportExportService
    {
        private readonly ILocalisationReaderService _localisationReaderService;
        private readonly IConfigurationReaderService _configurationReaderService;
        private readonly ILookupReaderService _lookupReaderService;
        private readonly IBlobServiceFactory _blobServiceFactory;
        private readonly RequestContext _requestContext;
        private readonly ISystemDataContext _systemDataContext;
        private readonly IAlertNotificationRequestService _alertNotificationRequestService;

        public ReportExportService(ILocalisationReaderService localisationReaderService, IConfigurationReaderService configurationReaderService,
            ILookupReaderService lookupReaderService, IBlobServiceFactory blobServiceFactory,
            RequestContext requestContext, ISystemDataContext systemDataContext, IAlertNotificationRequestService alertNotificationRequestService)
        {
            _localisationReaderService = localisationReaderService;
            _configurationReaderService = configurationReaderService;
            _lookupReaderService = lookupReaderService;
            _blobServiceFactory = blobServiceFactory;
            _requestContext = requestContext;
            _systemDataContext = systemDataContext;
            _alertNotificationRequestService = alertNotificationRequestService;
        }

        public async Task ExportAndNotifyAsync(string masterReportType, string reportType, IEnumerable<string> columnLocalisationKeys, IEnumerable<IEnumerable<object>> contentList)
        {
            var user = await _systemDataContext.GetDocumentAsync<User>(_requestContext.UserId);
            var expirySetting = await _configurationReaderService.GetValueAsync(SystemKeys.ConfigurationKeys.ReportExportExpiryHours);
            var expiryHours = int.Parse(expirySetting);

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
            var userDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);
            var reportName = await _lookupReaderService.GetTextAsync("LIST_" + masterReportType, reportType);
            var blobInfo = await ExportAndGenerateBlobAsync(reportName, columnLocalisationKeys, contentList, expiryHours, userDateTime);
            
            var reportAlertInfo = new ReportAlertInfo
            {
                ExpiryHours = expiryHours,
                GeneratedOnUtc = DateTime.UtcNow,
                ReportName = reportName,
                DownloadUrl = blobInfo.BlobUri + blobInfo.SasToken,
            };

            await _alertNotificationRequestService.NotifyAsync(
                SystemKeys.AlertTemplateKeys.ReportExport,
                SystemKeys.AlertChannelKeys.Email,
                new Dictionary<string, object>
                {
                    { AlertObjectContextTypes.Report, reportAlertInfo },
                    { AlertObjectContextTypes.User, user.Id }
                },
                null,
                user.Id);
        }

        private async Task<BlobSasTokenDto> ExportAndGenerateBlobAsync(string reportName, IEnumerable<string> columnLocalisationKeys, IEnumerable<IEnumerable<object>> contentList, int expiryHours, DateTime userDateTime)
        {
            var columnNames = new List<string>();
            foreach (var key in columnLocalisationKeys)
            {
                var text = await _localisationReaderService.GetTextAsync(key);
                columnNames.Add(text);
            }

            var templateFile = ExportToTemplateFile(reportName, columnNames, contentList);
            
            //Create the report file inside a GUID folder.
            var blobService = await _blobServiceFactory.CreateBlobServiceAsync("reportexports", Guid.NewGuid().ToString("N"));
            var blobId = userDateTime.ToStandardReportNameFormatByDate(reportName, "xlsx");

            using (var outStream = await blobService.OpenWriteBlobAsync(blobId))
            {
                templateFile.Write(outStream);
            }

            var sasToken = blobService.GenerateSasForBlob(blobId, TimeSpan.FromHours(expiryHours), BlobSasPermissions.Read);
            return sasToken;
        }

        private ITemplateFile ExportToTemplateFile(string reportName, IEnumerable<string> columnNames, IEnumerable<IEnumerable<object>> contentList)
        {
            var templateFile = new TemplateFile();
            var sheet = templateFile.CreateTemplateSheetDataStore(reportName, 1);

            //Create data rows with associated column names to be fed into the sheet.
            var dataRows = new List<Dictionary<string, object>>();
            foreach (var content in contentList)
            {
                var dataRow = new Dictionary<string, object>();

                for (var i = 0; i < columnNames.Count(); i++)
                {
                    var colName = columnNames.ElementAt(i).ToLower();
                    object value = content.ElementAtOrDefault(i);
                    dataRow[colName] = value;
                }

                dataRows.Add(dataRow);
            }

            sheet.WriteColumnNames(columnNames);
            sheet.WriteDataRows(dataRows);

            return templateFile;
        }
    }
}
