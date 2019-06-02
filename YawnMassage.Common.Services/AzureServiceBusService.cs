using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto.ServiceBus;
using YawnMassage.Common.Domain.Exceptions;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    [ExcludeFromCodeCoverage]
    public class AzureServiceBusService : IServiceBusService
    {
        private readonly IConfigurationReaderService _configurationService;
        private IQueueClient _queueClient;

        public AzureServiceBusService(IConfigurationReaderService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task SendMessageAsync(ServiceBusMessageDto messageDto, string queueName)
        {
            _queueClient = await CreateQueueClientAsync(queueName);
            var message = CreateMessage(messageDto);

            try
            {
                await _queueClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new YawnMassageException("ERROR_NOTIFYING_SERVICE_BUS", ex);
            }
        }

        private Message CreateMessage(ServiceBusMessageDto messageDto)
        {
            var message = string.IsNullOrWhiteSpace(messageDto.MessagePayload) ? new Message() : new Message(Encoding.UTF8.GetBytes(messageDto.MessagePayload));

            if (messageDto.CustomProperties != null)
            {
                foreach (KeyValuePair<string, object> property in messageDto.CustomProperties)
                {
                    message.UserProperties.Add(property.Key, property.Value);
                }
            }

            return message;
        }

        private async Task<QueueClient> CreateQueueClientAsync(string queueName)
        {
            var serviecBusConnectionString = await _configurationService.GetValueAsync(SystemKeys.ConfigurationKeys.ServiceBusConnectionString);

            return new QueueClient(serviecBusConnectionString, queueName);
        }
    }


}
