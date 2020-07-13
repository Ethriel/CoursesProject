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
        private readonly ISendEmailService _sendEmail;
        private readonly IUrlHelper _urlHelper;

        public EmailService(ISendEmailService sendEmail, IUrlHelperFactory urlHelperFactory,
                   IActionContextAccessor actionContextAccessor)
        {
            _sendEmail = sendEmail;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        public async Task SendConfirmMessageAsync(int userId, string token, string email, string protocol)
        {
            var callbackUrl = _urlHelper.Action("ConfirmEmail", "Account", new { userId = userId, token = token }, protocol: protocol);
            var subject = "Confirm your email";
            var message = "Confirm your email, please, by clicking on the link: " +
                $"<a href='{callbackUrl}'>Confirm</a>";
            await _sendEmail.SendEmailAsync(email, subject, message);
        }

        public async Task SendNotifyMessageAsync()
        {
            throw new NotImplementedException();
        }
    }
}
