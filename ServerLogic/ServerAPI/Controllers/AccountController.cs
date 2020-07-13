using Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SystemUserDTO userData)
        {
            try
            {
                var data = await _accountService.SignUpAsync(userData, HttpContext);
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
                var data = await _accountService.SignInAsync(userData);
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
                var data = await _accountService.UseFacebookAsync(facebookUser);
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
                var data = await _accountService.ConfirmEmailAsync(userId, token);
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