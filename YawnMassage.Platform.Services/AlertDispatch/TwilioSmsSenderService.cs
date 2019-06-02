using YawnMassage.Platform.Domain.Contracts.AlertDispatch;
using YawnMassage.Platform.Domain.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.AlertDispatch
{
    public class TwilioSmsSenderService : IAlertSenderService
    {
        public TwilioSmsSenderService()
        {

        }

        public async Task SendAsync(string content, string subject, string senderId, User recipientUser)
        {
            await Task.CompletedTask;
        }
    }
}
