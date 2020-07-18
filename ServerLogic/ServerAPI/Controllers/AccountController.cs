using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
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

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var result = await accountService.ConfirmEmailAsync(userId, token);

            return this.GetActionResult(result, logger);
        }
    }
}