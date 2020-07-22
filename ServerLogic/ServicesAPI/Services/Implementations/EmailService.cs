using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ISendEmailService sendEmail;
        private readonly IUrlHelper urlHelper;

        public EmailService(ISendEmailService sendEmail, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            this.sendEmail = sendEmail;
            urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        public async Task SendConfirmMessageAsync(int userId, string token, string email, string protocol)
        {
            var callbackUrl = GetCallbackUrl("ConfirmEmail", new { userId = userId, token = token }, protocol);
            //var callbackUrl = urlHelper.Action("ConfirmEmail", "Account", new { userId = userId, token = token }, protocol: protocol);
            await SendConfirmEmailAsync(callbackUrl, email);
        }

        public void SendNotifyMessage(string email, string courseDetails)
        {
            var subject = "Course start notification";
            var message = courseDetails;
            sendEmail.SendEmail(email, subject, message);
        }

        public async Task SendConfirmChangeEmailAsync(int userId, string token, string email, string protocol)
        {
            var callbackUrl = GetConfirmChangeEmailUrl(new { userId = userId, email = email, token = token }, protocol);
            await SendConfirmEmailAsync(callbackUrl, email);
        }
        private string GetCallbackUrl(string action, object data, string protocol)
        {
            var callbackUrl = urlHelper.Action(action, "Account", data, protocol);
            return callbackUrl;
        }
        private string GetConfirmChangeEmailUrl(object data, string protocol)
        {
            var callbackUrl = urlHelper.RouteUrl("ConfirmChangeEmail", data, protocol);
            return callbackUrl;
        }
        private async Task SendConfirmEmailAsync(string callbackUrl, string email)
        {
            var subject = GetConfirmSubject();
            var message = GetConfirmMessage(callbackUrl);
            await sendEmail.SendEmailAsync(email, subject, message);
        }
        private string GetConfirmSubject()
        {
            return "Confirm your email";
        }
        private string GetConfirmMessage(string callbackUrl)
        {
            return $"Confirm your email, please, by clicking on the link: <a href='{callbackUrl}'>Confirm</a>";
        }
    }
}
