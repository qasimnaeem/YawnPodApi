using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.Alerts;
using YawnMassage.Common.Domain.Dto.ServiceBus;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    public class AlertNotificationRequestService : IAlertNotificationRequestService
    {
        private readonly RequestContext _requestContext;
        private readonly QueueNameInfo _queueNameInfo;
        private readonly IServiceBusService _serviceBusService;

        public AlertNotificationRequestService(RequestContext requestContext, QueueNameInfo queueNameInfo, IServiceBusService serviceBusService)
        {
            _requestContext = requestContext;
            _queueNameInfo = queueNameInfo;
            _serviceBusService = serviceBusService;
        }

        public async Task NotifyAsync(string templateKey, string channelKey, Dictionary<string, object> contextObjectinfos, Dictionary<string, string> customParamReplacements, params string[] recipientUserIds)
        {
            //Queue messages per each recipient user.
            foreach (var recipientUserId in recipientUserIds)
            {
                var payload = new AlertNotificationRequestDto
                {
                    TemplateKey = templateKey,
                    ChannelKey = channelKey,
                    ContextObjectInfos = contextObjectinfos,
                    CustomParamReplacements = customParamReplacements,
                    RecipientUserId = recipientUserId
                };

                await _serviceBusService.SendMessageAsync(new ServiceBusMessageDto
                {
                    MessagePayload = JsonConvert.SerializeObject(payload),
                    CustomProperties = new Dictionary<string, object>
                    {
                        { ServiceBusMessageProperties.GroupId, _requestContext.GroupId },
                        { ServiceBusMessageProperties.UserId, _requestContext.UserId ?? string.Empty }
                    }
                }, _queueNameInfo.AlertNotifyQueueName);
            }
        }
    }
}
