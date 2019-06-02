using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Documents;
namespace YawnMassage.Platform.Domain.Contracts.AlertDispatch
{
    /// <summary>
    /// Defines the <see cref="IAlertSenderService" />
    /// </summary>
    public interface IAlertSenderService
    {
        /// <summary>
        /// The SendAsync
        /// </summary>
        /// <param name="content">The content<see cref="string"/></param>
        /// <param name="subject">The subject<see cref="string"/></param>
        /// <param name="senderId">The senderId<see cref="string"/></param>
        /// <param name="recipientUser">The recipientUser<see cref="User"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task SendAsync(string content, string subject, string senderId, User recipientUser);
    }
}
