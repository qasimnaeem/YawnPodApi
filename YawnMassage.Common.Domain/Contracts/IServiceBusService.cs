using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto.ServiceBus;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IServiceBusService" />
    /// </summary>
    public interface IServiceBusService
    {
        /// <summary>
        /// The SendMessageAsync
        /// </summary>
        /// <param name="messageDto">The messageDto<see cref="ServiceBusMessageDto"/></param>
        /// <param name="queueName">The queueName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task SendMessageAsync(ServiceBusMessageDto messageDto, string queueName);
    }
}
