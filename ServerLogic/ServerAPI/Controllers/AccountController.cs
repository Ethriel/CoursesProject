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

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AccountController : Controller
    {
        private readonly CoursesSystemDbContext context;
        private readonly SignInManager<SystemUser> signInManager;
        private readonly RoleManager<SystemRole> roleManager;
        private readonly UserManager<SystemUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SecurityTokenHandler tokenValidator;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;
        private readonly IHttpClientFactory httpClientFactory;

        public AccountController(CoursesSystemDbContext context, SignInManager<SystemUser> signInManager,
            RoleManager<SystemRole> roleManager,
            UserManager<SystemUser> userManager,
            IConfiguration configuration,
            SecurityTokenHandler tokenValidator,
            IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper,
            IHttpClientFactory httpClientFactory)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.tokenValidator = tokenValidator;
            this.mapperWrapper = mapperWrapper;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register(SystemUserDTO userData)
        {
            var user = mapperWrapper.MapFromDTO(userData);
            user.UserName = userData.Email;
            user.RegisteredDate = DateTime.Now;
            var role = await roleManager.FindByNameAsync("USER");
            user.SystemRole = role;
            user.CalculateAge();
            var res = await userManager.CreateAsync(user, userData.Password);
            if (res.Succeeded)
            {
                var data = UserResponseHelper.GetResponseData(configuration, tokenValidator, mapperWrapper, user);
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
            var user = await context.Users.Include(x => x.SystemRole).FirstOrDefaultAsync(user => user.Email.Equals(userData.Email));
            var res = await signInManager.PasswordSignInAsync(user, userData.Password, true, false);
            if (res.Succeeded)
            {
                var data = UserResponseHelper.GetResponseData(configuration, tokenValidator, mapperWrapper, user);
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
            var addId = configuration["Authentication:Facebook:AppId"].Replace("<", "").Replace(">", "");
            var secret = configuration["Authentication:Facebook:AppSecret"].Replace("<", "").Replace(">", "");
            var formattedUrl = string.Format(VALIDATION_URL, facebookUser.AccessToken, addId, secret);
            var validatinResult = await httpClientFactory.CreateClient().GetAsync(formattedUrl);
            validatinResult.EnsureSuccessStatusCode();
            var responseAsString = await validatinResult.Content.ReadAsStringAsync();

            var faceBookValidation = JsonConvert.DeserializeObject<FacebookTokenValidator>(responseAsString);
            var isTokenValid = faceBookValidation.Data.IsValid;
            if (!isTokenValid)
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = "INVALID LOGIN ATTEMPT", errors = errors });
            }
            else
            {
                var user = await userManager.FindByEmailAsync(facebookUser.Email);
                if (user == null)
                {
                    var role = await roleManager.FindByNameAsync("USER");
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
                    var createResult = await userManager.CreateAsync(systemUser);
                    if (createResult.Succeeded)
                    {
                        systemUser = await context.SystemUsers.FirstOrDefaultAsync(x => x.Email.Equals(facebookUser.Email));
                        var data = UserResponseHelper.GetResponseData(configuration, tokenValidator, mapperWrapper, systemUser);
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
                    var data = UserResponseHelper.GetResponseData(configuration, tokenValidator, mapperWrapper, user);
                    return Ok(data);
                }
            }
            
        }
    }
}
