using System.Collections.Generic;
namespace YawnMassage.Common.Domain.Dto.ServiceBus
{
    /// <summary>
    /// Defines the <see cref="ServiceBusMessageDto" />
    /// </summary>
    public class ServiceBusMessageDto
    {
        /// <summary>
        /// Gets or sets the MessagePayload
        /// </summary>
        public string MessagePayload { get; set; }

        /// <summary>
        /// Gets or sets the CustomProperties
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }
    }
}
