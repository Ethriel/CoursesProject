using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using ServerAPI.Services.Abstractions;
using System;
using System.Threading.Tasks;

namespace ServerAPI.Services.Implementations
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
            var callbackUrl = urlHelper.Action("ConfirmEmail", "Account", new { userId = userId, token = token }, protocol: protocol);
            var subject = "Confirm your email";
            var message = "Confirm your email, please, by clicking on the link: " +
                $"<a href='{callbackUrl}'>Confirm</a>";
            await sendEmail.SendEmailAsync(email, subject, message);
        }

        public void SendNotifyMessage(string email, string courseDetails)
        {
            var subject = "Course start notification";
            var message = courseDetails;
            sendEmail.SendEmail(email, subject, message);
        }
    }
}
