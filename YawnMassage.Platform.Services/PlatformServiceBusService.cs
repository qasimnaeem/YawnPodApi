using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Platform.Domain.Dto.User;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ServiceBus;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    [ExcludeFromCodeCoverage]
    public class PlatformServiceBusService : IPlatformServiceBusService
    {
        private readonly IServiceBusService _serviceBusService;
        private readonly QueueNameInfo _appSettingsQueueNameInfo;
        private readonly RequestContext _requestContext;

        public PlatformServiceBusService(IServiceBusService serviceBusService, QueueNameInfo appSettingsQueueNameInfo,
            RequestContext requestContext)
        {
            _serviceBusService = serviceBusService;
            _appSettingsQueueNameInfo = appSettingsQueueNameInfo;
            _requestContext = requestContext;
        }

        public async Task TriggerPodAccessDefinitionGenerationAsync(UserUpdateMessageDto userUpdateMessageDto)
        {
            var msgDto = new ServiceBusMessageDto
            {
                MessagePayload = JsonConvert.SerializeObject(userUpdateMessageDto)
            };

            await _serviceBusService.SendMessageAsync(msgDto, _appSettingsQueueNameInfo.UserUpdateQueueName);
        }

        public async Task TriggerPodAccessDefinitionGenerationAsync(RoleUpdateMessageDto roleUpdateMessageDto)
        {
            var msgDto = new ServiceBusMessageDto
            {
                MessagePayload = JsonConvert.SerializeObject(roleUpdateMessageDto)
            };

            await _serviceBusService.SendMessageAsync(msgDto, _appSettingsQueueNameInfo.RoleUpdateQueueName);
        }

        public async Task TriggerReportExportAsync(string masterReportType, string reportType, object reportCriteria)
        {
            var serviceBusMessageDto = new ServiceBusMessageDto
            {
                MessagePayload = JsonConvert.SerializeObject(reportCriteria),
                CustomProperties = new Dictionary<string, object>
                {
                    { ServiceBusMessageProperties.MessageType, ServiceBusMessageTypes.ReportExport },
                    { ServiceBusMessageProperties.MasterReportType, masterReportType },
                    { ServiceBusMessageProperties.ReportType, reportType },
                    { ServiceBusMessageProperties.GroupId, _requestContext.GroupId },
                    { ServiceBusMessageProperties.UserId, _requestContext.UserId },
                    { ServiceBusMessageProperties.UserDisplayName, _requestContext.UserDisplayName },
                    { ServiceBusMessageProperties.Culture, _requestContext.Culture }
                }
            };

            await _serviceBusService.SendMessageAsync(serviceBusMessageDto, _appSettingsQueueNameInfo.ReportExportQueueName);
        }

        public async Task QueueBulkExportMessageAsync(string bulkDataOperstionID)
        {
            var serviceBusMessageDto = new ServiceBusMessageDto
            {
                CustomProperties = new Dictionary<string, object>
                {
                    { "BulkDataOperationId", bulkDataOperstionID },
                    { ServiceBusMessageProperties.UserId, _requestContext.UserId },
                }
            };

            await _serviceBusService.SendMessageAsync(serviceBusMessageDto, _appSettingsQueueNameInfo.BulkExportQueueName);
        }

        public async Task QueueBulkImportMessageAsync(string bulkDataOperationID)
        {
            var serviceBusMessageDto = new ServiceBusMessageDto
            {
                CustomProperties = new Dictionary<string, object>
                {
                    { "BulkDataOperationId", bulkDataOperationID },
                    { ServiceBusMessageProperties.UserId, _requestContext.UserId },
                    { ServiceBusMessageProperties.UserDisplayName, _requestContext.UserDisplayName },
                    { ServiceBusMessageProperties.Culture, _requestContext.Culture }
                }
            };

            await _serviceBusService.SendMessageAsync(serviceBusMessageDto, _appSettingsQueueNameInfo.BulkImportQueueName);
        }
    }
}
