using YawnMassage.Platform.Domain.Contracts.AlertDispatch;
using YawnMassage.Common.Domain.Contracts;
using System.Collections.Generic;

namespace YawnMassage.Platform.Services.AlertDispatch
{
    public class AlertContextObjectServiceFactory : IAlertContextObjectServiceFactory
    {
        private readonly Dictionary<string, IAlertContextObjectService> _contextObjectServices;

        public AlertContextObjectServiceFactory(Dictionary<string, IAlertContextObjectService> contextObjectServices)
        {
            _contextObjectServices = contextObjectServices;
        }

        public IAlertContextObjectService GetAlertContextObjectService(string objectType)
        {
            var objectService = _contextObjectServices[objectType];
            return objectService;
        }
    }
}
