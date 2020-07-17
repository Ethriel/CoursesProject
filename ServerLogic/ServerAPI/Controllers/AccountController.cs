using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAPI.DTO;
using ServicesAPI.Facebook;
using ServicesAPI.Responses;
using ServicesAPI.Responses.AccountResponseData;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;
using ServerAPI.Extensions;

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

            var loggerMessage = $"User {userData.Email} signed up";

            return this.GetActionResult(result, logger, loggerMessage);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SystemUserDTO userData)
        {
            var result = await accountService.SignInAsync(userData);

            var loggerMessage = $"User {userData.Email} signed in";

            return this.GetActionResult(result, logger, loggerMessage);
        }
        [HttpPost("signin-facebook")]
        public async Task<IActionResult> SignInFaceBook([FromBody] FacebookUser facebookUser)
        {
            var result = await accountService.UseFacebookAsync(facebookUser);

            var loggerMessage = $"User {facebookUser.Email} used Facebook";

            return this.GetActionResult(result, logger, loggerMessage);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var result = await accountService.ConfirmEmailAsync(userId, token);

            var user = result.Data as AccountData;

            var loggerMessage = $"User {user.User.Email} confirmed email";

            return this.GetActionResult(result, logger, loggerMessage);
        }
    }
}