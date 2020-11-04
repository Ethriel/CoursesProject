using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
using ServicesAPI.DataPresentation.AccountManagement;
using ServicesAPI.DTO;
using ServicesAPI.Facebook;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly ILogger<AccountController> logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SystemUserDTO userData)
        {
            var result = await accountService.SignUpAsync(userData);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SystemUserDTO userData)
        {
            var result = await accountService.SignInAsync(userData);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut(EmailWrapper emailWrapper)
        {
            var result = await accountService.SignOutAsync(emailWrapper);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("signin-facebook")]
        public async Task<IActionResult> SignInFaceBook([FromBody] FacebookUser facebookUser)
        {
            var result = await accountService.UseFacebookAsync(facebookUser);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountUpdateData accountUpdateData)
        {
            var result = await accountService.UpdateAccountAsync(accountUpdateData);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailData confirmEmailData)
        {
            var result = await accountService.ConfirmEmailAsync(confirmEmailData);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("checkEmailConfirmed")]
        public async Task<IActionResult> CheckEmailConfirmed([FromBody] EmailWrapper emailWrapper)
        {
            var result = await accountService.CheckEmailConfirmedAsync(emailWrapper);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("confirmEmailRequest")]
        public async Task<IActionResult> ConfirmEmailRequest([FromBody] EmailWrapper emailWrapper)
        {
            var result = await accountService.ConfirmEmailRequestAsync(emailWrapper);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("confirmChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody] ConfirmChangeEmailData confirmChangeEmail)
        {
            var result = await accountService.ConfirmChangeEmailAsync(confirmChangeEmail);

            return this.GetActionResult(result, logger);
        }

        [HttpGet("verifyEmail/{email}")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            var result = await accountService.VerifyEmailAsync(email);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailWrapper emailWrapper)
        {
            var result = await accountService.ForgotPasswordAsync(emailWrapper);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordData resetPasswordData)
        {
            var result = await accountService.ResetPasswordAsync(resetPasswordData);

            return this.GetActionResult(result, logger);
        }
    }
}