using System.Collections.Generic;
using System.Threading.Tasks;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IAlertNotificationRequestService" />
    /// </summary>
    public interface IAlertNotificationRequestService
    {
        /// <summary>
        /// The NotifyAsync
        /// </summary>
        /// <param name="templateKey">The templateKey<see cref="string"/></param>
        /// <param name="channelKey">The channelKey<see cref="string"/></param>
        /// <param name="contextObjectinfos">The contextObjectinfos<see cref="Dictionary{string, object}"/></param>
        /// <param name="customParamReplacements">The customParamReplacements<see cref="Dictionary{string, string}"/></param>
        /// <param name="recipientUserIds">The recipientUserIds<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task NotifyAsync(string templateKey, string channelKey, Dictionary<string, object> contextObjectinfos, Dictionary<string, string> customParamReplacements, params string[] recipientUserIds);
    }
}
