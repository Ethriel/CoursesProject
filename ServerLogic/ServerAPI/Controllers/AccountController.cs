using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAPI.DTO;
using ServicesAPI.Facebook;
using ServicesAPI.Responses.AccountResponseData;
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
            var data = await accountService.SignUpAsync(userData, HttpContext);

            logger.LogInformation($"User {data.AccountData.User.Email} signed up");
            switch (data.AccountOperationResult)
            {
                case AccountOperationResult.Succeeded:
                    return Ok(new { data = data.AccountData });
                case AccountOperationResult.Failed:
                default:
                    return BadRequest(new { message = "Sign up failed" });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SystemUserDTO userData)
        {
            var data = await accountService.SignInAsync(userData);

            logger.LogInformation($"User {data.AccountData.User.Email} signed in");

            switch (data.AccountOperationResult)
            {
                case AccountOperationResult.Succeeded:
                    return Ok(new { data = data.AccountData });
                case AccountOperationResult.Failed:
                default:
                    return BadRequest(new { message = "Sign in failed" });
            }
        }
        [HttpPost("signin-facebook")]
        public async Task<IActionResult> SignInFaceBook([FromBody] FacebookUser facebookUser)
        {
            var data = await accountService.UseFacebookAsync(facebookUser);

            logger.LogInformation($"User {data.AccountData.User.Email} signed in with Facebook");

            switch (data.AccountOperationResult)
            {
                case AccountOperationResult.Succeeded:
                    return Ok(new { data = data.AccountData });
                case AccountOperationResult.Failed:
                default:
                    return BadRequest(new { message = "Sign in with Facebook failed" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var data = await accountService.ConfirmEmailAsync(userId, token);

            switch (data.AccountOperationResult)
            {
                case AccountOperationResult.Succeeded:
                    return Ok(new { message = "Email confirmed" });
                case AccountOperationResult.Failed:
                default:
                    return BadRequest(new { message = "Email confirmation failed" });
            }
        }
    }
}