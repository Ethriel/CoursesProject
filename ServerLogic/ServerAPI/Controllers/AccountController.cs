using Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
using ServerAPI.Facebook;
using ServerAPI.Responses;
using ServerAPI.Services.Abstractions;
using System;
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
                var accresp = data as AccountResponse;
                logger.LogInformation($"User {accresp.User.Email} signed up");
                return this.GetCorrespondingResponse(data);
            }
            catch (Exception ex)
            {
                var errors = this.GetErrorsFromModelState();
                var error = new ErrorObject("Invalid sign up attempt", errors);
                return BadRequest(error);
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SystemUserDTO userData)
        {
            try
            {
                var data = await accountService.SignInAsync(userData);
                var accresp = data as AccountResponse;
                logger.LogInformation($"User {accresp.User.Email} signed in");
                return this.GetCorrespondingResponse(data);
            }
            catch (Exception ex)
            {
                var errors = this.GetErrorsFromModelState();
                var error = new ErrorObject("Invalid sign in attempt", errors);
                return BadRequest(error);
            }
        }
        [HttpPost("signin-facebook")]
        public async Task<IActionResult> SignInFaceBook([FromBody] FacebookUser facebookUser)
        {
            try
            {
                var data = await accountService.UseFacebookAsync(facebookUser);
                return this.GetCorrespondingResponse(data);
            }
            catch (Exception ex)
            {
                var errors = this.GetErrorsFromModelState();
                var error = new ErrorObject("Invalid facebook login", errors);
                return BadRequest(error);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            try
            {
                var data = await accountService.ConfirmEmailAsync(userId, token);
                return this.GetCorrespondingResponse(data);
            }
            catch (Exception ex)
            {
                var errors = this.GetErrorsFromModelState();
                var error = new ErrorObject("An exception occured", errors);
                return BadRequest(error);
            }
        }
    }
}