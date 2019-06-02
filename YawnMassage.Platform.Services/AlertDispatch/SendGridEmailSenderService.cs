using YawnMassage.Platform.Domain.Contracts.AlertDispatch;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.AlertDispatch
{
    public class SendGridEmailSenderService : IAlertSenderService
    {
        private readonly IConfigurationReaderService _configurationReaderService;

        public SendGridEmailSenderService(IConfigurationReaderService configurationReaderService)
        {
            _configurationReaderService = configurationReaderService;
        }

        public async Task SendAsync(string content, string subject, string senderId, User recipientUser)
        {
            var from = new EmailAddress(senderId);
            var to = new EmailAddress(recipientUser.Email);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, content);
            
            var apiKey = await _configurationReaderService.GetValueAsync(SystemKeys.ConfigurationKeys.SendGridApiKey);
            var _sendGridClient = new SendGridClient(apiKey);

            await _sendGridClient.SendEmailAsync(msg);
        }
    }
}
