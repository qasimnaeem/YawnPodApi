using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto.Alerts;

namespace YawnMassage.Platform.Domain.Contracts.AlertDispatch
{
    /// <summary>
    /// Defines the <see cref="IAlertDispatcherService" />
    /// </summary>
    public interface IAlertDispatcherService
    {
        /// <summary>
        /// The DispatchAlertAsync
        /// </summary>
        /// <param name="alertRequest">The alertRequest<see cref="AlertNotificationRequestDto"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DispatchAlertAsync(AlertNotificationRequestDto alertRequest);
    }
}
