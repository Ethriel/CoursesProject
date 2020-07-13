using ServerAPI.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private const string CONFIRM_URL = "https://localhost:44382/account/confirm/{0}/{1}";
        private readonly ISendEmailService _sendEmail;

        public EmailService(ISendEmailService sendEmail)
        {
            _sendEmail = sendEmail;
        }
        public async Task SendConfirmMessageAsync(int userId, string token, string email)
        {
            var url = string.Format(CONFIRM_URL, userId, token);
            var subject = "Confirm your email";
            var message = "Confirm your email, please, by clicking on the link:\n" +
                $"<a href='{url}'>Confirm</a>";
            await _sendEmail.SendEmailAsync(email, subject, message);
        }

        public async Task SendNotifyMessageAsync()
        {
            throw new NotImplementedException();
        }
    }
}
