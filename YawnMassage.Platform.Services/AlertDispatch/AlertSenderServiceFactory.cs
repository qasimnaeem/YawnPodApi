using YawnMassage.Platform.Domain.Contracts.AlertDispatch;
using System.Collections.Generic;

namespace YawnMassage.Platform.Services.AlertDispatch
{
    public class AlertSenderServiceFactory : IAlertSenderServiceFactory
    {
        private readonly Dictionary<string, IAlertSenderService> _senderServices;

        public AlertSenderServiceFactory(Dictionary<string, IAlertSenderService> senderServices)
        {
            _senderServices = senderServices;
        }

        public IAlertSenderService GetAlertSenderService(string channel)
        {
            var service = _senderServices[channel];
            return service;
        }
    }
}
