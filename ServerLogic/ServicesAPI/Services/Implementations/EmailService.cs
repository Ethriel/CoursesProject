using Microsoft.Extensions.Configuration;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ISendEmailService sendEmail;
        private readonly IConfiguration configuration;

        public EmailService(ISendEmailService sendEmail,
                            IConfiguration configuration)
        {
            this.sendEmail = sendEmail;
            this.configuration = configuration;
        }

        public async Task SendConfirmMessageAsync(string token, string email)
        {
            var clientRoute = "confirmEmail";

            await SendConfirmEmailAsync(clientRoute, token, email);
        }

        public async Task SendConfirmChangeEmailAsync(string token, string email)
        {
            var clientRoute = "confirmChangeEmail";

            await SendConfirmEmailAsync(clientRoute, token, email);
        }
        public async Task SendResetPasswordData(string token, string email)
        {
            //&email ={ email}
            var subject = "Reset password";
            var client = configuration["client"];
            var clientRoute = "resetPassword";
            var callbackUrl = $"{client}/{clientRoute}?token={token}";
            var message = GetResetPasswordMessage(callbackUrl);

            await sendEmail.SendEmailAsync(email, subject, message);
        }

        public void SendNotifyMessage(string email, string courseDetails)
        {
            var subject = "Course start notification";
            var message = courseDetails;
            sendEmail.SendEmail(email, subject, message);
        }

        private async Task SendConfirmEmailAsync(string clientRoute, string token, string email)
        {
            var subject = GetConfirmSubject();
            var callbackUrl = GetCallbackUrl(clientRoute, token);
            var message = GetConfirmMessage(callbackUrl);
            await sendEmail.SendEmailAsync(email, subject, message);
        }

        private string GetCallbackUrl(string clientRoute, string token)
        {
            var client = configuration["client"];
            var callbackUrl = $"{client}/{clientRoute}?token={token}";
            return callbackUrl;
        }

        private string GetConfirmSubject()
        {
            return "Confirm your email";
        }

        private string GetConfirmMessage(string callbackUrl)
        {
            var link = GetLink(callbackUrl, "Confirm");
            return $"Confirm your email, please, by clicking on the link: {link}";
        }
        private string GetResetPasswordMessage(string callbackUrl)
        {
            var link = GetLink(callbackUrl, "Reset");
            return $"Reset your password by clicking the link: {link}";
        }
        private string GetLink(string callbackUrl, string message)
        {
            return $"<a href='{callbackUrl}'>{message}</a>";
        }
    }
}
