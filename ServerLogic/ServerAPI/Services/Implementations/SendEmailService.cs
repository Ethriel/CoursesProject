using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ServerAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServerAPI.Services.Implementations
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _configuration;

        public SendEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var format = subject.Contains("Confirm") ? MimeKit.Text.TextFormat.Html : MimeKit.Text.TextFormat.Plain;
            var sender = _configuration["Email:User"];
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Courses administration", sender));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(format)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587);
                await client.AuthenticateAsync(sender, _configuration["Email:Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        public void SendEmail(string email, string subject, string message)
        {
            var format = subject.Contains("Confirm") ? MimeKit.Text.TextFormat.Html : MimeKit.Text.TextFormat.Plain;
            var sender = _configuration["Email:User"];
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Courses administration", sender));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(format)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);
                client.Authenticate(sender, _configuration["Email:Password"]);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
    }
}
