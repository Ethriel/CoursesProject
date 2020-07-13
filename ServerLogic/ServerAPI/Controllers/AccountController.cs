using Infrastructure.DbContext;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.Facebook;
using ServerAPI.Helpers;
using ServerAPI.MapperWrappers;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using ServerAPI.Services.Abstractions;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AccountController : Controller
    {
        private readonly CoursesSystemDbContext _context;
        private readonly SignInManager<SystemUser> _signInManager;
        private readonly RoleManager<SystemRole> _roleManager;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SecurityTokenHandler _tokenValidator;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> _mapperWrapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;

        public AccountController(CoursesSystemDbContext context, SignInManager<SystemUser> signInManager,
            RoleManager<SystemRole> roleManager,
            UserManager<SystemUser> userManager,
            IConfiguration configuration,
            SecurityTokenHandler tokenValidator,
            IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper,
            IHttpClientFactory httpClientFactory,
            IEmailService emailService)
        {
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenValidator = tokenValidator;
            _mapperWrapper = mapperWrapper;
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] SystemUserDTO userData)
        {
            var user = _mapperWrapper.MapFromDTO(userData);
            user.UserName = userData.Email;
            user.RegisteredDate = DateTime.Now;
            var role = await _roleManager.FindByNameAsync("USER");
            user.SystemRole = role;
            user.CalculateAge();
            var res = await _userManager.CreateAsync(user, userData.Password);
            if (res.Succeeded)
            {
                user = await _context.SystemUsers.FirstOrDefaultAsync(x => x.Email.Equals(userData.Email));
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                try
                {
                    await _emailService.SendConfirmMessageAsync(user.Id, token, user.Email);
                }
                catch (Exception ex)
                {
                    throw;
                }
                var data = UserResponseHelper.GetResponseData(_configuration, _tokenValidator, _mapperWrapper, user);
                return Ok(data);
            }
            else
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = "INVALID REGISTRATION ATTEMPT", errors = errors });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SystemUserDTO userData)
        {
            var user = await _context.Users.Include(x => x.SystemRole).FirstOrDefaultAsync(user => user.Email.Equals(userData.Email));
            var res = await _signInManager.PasswordSignInAsync(user, userData.Password, true, false);
            if (res.Succeeded)
            {
                var data = UserResponseHelper.GetResponseData(_configuration, _tokenValidator, _mapperWrapper, user);
                return Ok(data);
            }
            else
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = "INVALID LOGIN ATTEMPT", errors = errors });
            }
        }
        [HttpPost("signin-facebook")]
        public async Task<IActionResult> SignInFaceBook([FromBody] FacebookUserObject facebookUser)
        {
            const string VALIDATION_URL = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
            var addId = _configuration["Authentication:Facebook:AppId"];
            var secret = _configuration["Authentication:Facebook:AppSecret"];
            var formattedUrl = string.Format(VALIDATION_URL, facebookUser.AccessToken, addId, secret);
            var validationResult = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            validationResult.EnsureSuccessStatusCode();
            var responseAsString = await validationResult.Content.ReadAsStringAsync();

            var facebookTokenData = JsonConvert.DeserializeObject<FacebookTokenData>(responseAsString);
            var isTokenValid = facebookTokenData.Data.IsValid;
            if (!isTokenValid)
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = "INVALID LOGIN ATTEMPT", errors = errors });
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(facebookUser.Email);
                if (user == null)
                {
                    var role = await _roleManager.FindByNameAsync("USER");
                    var systemUser = new SystemUser
                    {
                        FirstName = facebookUser.FirstName,
                        LastName = facebookUser.LastName,
                        Email = facebookUser.Email,
                        UserName = facebookUser.Email,
                        EmailConfirmed = true,
                        SystemRole = role,
                        AvatarPath = facebookUser.PictureUrl
                    };
                    var createResult = await _userManager.CreateAsync(systemUser);
                    if (createResult.Succeeded)
                    {
                        systemUser = await _context.SystemUsers.FirstOrDefaultAsync(x => x.Email.Equals(facebookUser.Email));
                        var data = UserResponseHelper.GetResponseData(_configuration, _tokenValidator, _mapperWrapper, systemUser);
                        return Ok(data);
                    }
                    else
                    {
                        var createErrors = createResult.Errors;
                        var modelErrors = GetErrorsFromModelState.GetErrors(ModelState);
                        return BadRequest(new { message = "Somethig went wrong", createErrors = createErrors, modelErrors = modelErrors });
                    }
                }
                else
                {
                    var data = UserResponseHelper.GetResponseData(_configuration, _tokenValidator, _mapperWrapper, user);
                    return Ok(data);
                }
            }

        }
        
        [HttpPost("confirm/{userId}/{token}")]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var user = await _context.SystemUsers.FindAsync(userId);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (confirmResult.Succeeded)
            {
                return Ok("Email confirmed");
            }
            else
            {
                return BadRequest(new { message = "Confirmation failed", confirmResult.Errors });
            }
        }
    }
}
