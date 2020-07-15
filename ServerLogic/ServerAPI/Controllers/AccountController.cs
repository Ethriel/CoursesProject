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
    [Route("[controller]")]

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
            try
            {
                var data = await accountService.SignUpAsync(userData, HttpContext);

                logger.LogInformation($"User {data.AccountData.User.Email} signed up");
                switch (data.AccountOperationResult)
                {
                    case AccountOperationResult.Succeeded:
                        return Ok(new { data.AccountData });
                    case AccountOperationResult.Failed:
                    default:
                        return BadRequest(new { message = "Sign up failed" });
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SystemUserDTO userData)
        {
            try
            {
                var data = await accountService.SignInAsync(userData);

                logger.LogInformation($"User {data.AccountData.User.Email} signed in");

                switch (data.AccountOperationResult)
                {
                    case AccountOperationResult.Succeeded:
                        return Ok(new { data.AccountData });
                    case AccountOperationResult.Failed:
                    default:
                        return BadRequest(new { message = "Sign in failed" });
                }
            }
            catch
            {
                throw;
            }
        }
        [HttpPost("signin-facebook")]
        public async Task<IActionResult> SignInFaceBook([FromBody] FacebookUser facebookUser)
        {
            try
            {
                var data = await accountService.UseFacebookAsync(facebookUser);

                logger.LogInformation($"User {data.AccountData.User.Email} signed in with Facebook");

                switch (data.AccountOperationResult)
                {
                    case AccountOperationResult.Succeeded:
                        return Ok(new { data.AccountData });
                    case AccountOperationResult.Failed:
                    default:
                        return BadRequest(new { message = "Sign in with Facebook failed" });
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            try
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
            catch
            {
                throw;
            }
        }
    }
}