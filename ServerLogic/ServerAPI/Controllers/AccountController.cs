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
            var result = await accountService.SignUpAsync(userData, HttpContext);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SystemUserDTO userData)
        {
            var result = await accountService.SignInAsync(userData);

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
            var result = await accountService.UpdateAccountAsync(accountUpdateData, HttpContext);

            return this.GetActionResult(result, logger);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var result = await accountService.ConfirmEmailAsync(userId, token);

            return this.GetActionResult(result, logger);
        }
        [Route("confirmChange", Name = "ConfirmChangeEmail")]
        [HttpGet]
        public async Task<IActionResult> ChangeEmail(int userId, string email, string token)
        {
            var result = await accountService.ConfirmChangeEmailAsync(userId, email, token);

            return this.GetActionResult(result, logger);
        }
    }
}