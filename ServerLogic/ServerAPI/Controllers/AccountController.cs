using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using ServerAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AccountController : Controller
    {
        public CoursesSystemDbContext context { get; }
        public UserManager<SystemUser> userManager { get; }
        public SignInManager<SystemUser> signInManager { get; }
        public IConfiguration configuration { get; }
        public SecurityTokenHandler tokenValidator { get; }

        public AccountController(CoursesSystemDbContext context, 
            UserManager<SystemUser> userManager, 
            SignInManager<SystemUser> signInManager, 
            IConfiguration configuration,
            SecurityTokenHandler tokenValidator)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.tokenValidator = tokenValidator;
        }

        [Authorize]
        [HttpPost("protected")]
        public async Task<IActionResult> Register(/*SystemUserViewModel userData*/)
        {
            #region TEST
            //var role = Context.SystemRoles.FirstOrDefault(x => x.NormalizedName.Equals("User"));
            //var user = new SystemUser
            //{
            //    Email = "testuser@gmail.com",
            //    EmailConfirmed = true,
            //    UserName = "TESTUSER",
            //    NormalizedUserName = "testuser",
            //    FirstName = "Test",
            //    LastName = "User",
            //    RegisteredDate = DateTime.Now,
            //    BirthDate = new DateTime(1992, 5, 16),
            //    StudyDate = DateTime.Now.AddDays(30),
            //    AvatarPath = "wwwroot/shared/img/testuserpic.png",
            //    SystemRole = role
            //};
            //user.CalculateAge();
            //var result = await UserManager.CreateAsync(user, "Zx%c6v");

            //if (result.Succeeded)
            //{
            //    return Ok();
            //}
            //return BadRequest();
            #endregion



            return Ok("secure_text");
        }

        //[Authorize]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SystemUserDTO userData)
        {
            var user = await context.SystemUsers.Include(x => x.SystemRole).FirstOrDefaultAsync(x => x.Email.Equals(userData.Email));
            var res = await signInManager.PasswordSignInAsync(user, userData.Password, true, false);
            if (res.Succeeded)
            {
                var token = JWTHelper.GenerateJwtToken(user, configuration, tokenValidator);
                var data = new { email = user.Email, userRole = user.SystemRole.Name, access_token = token, expires = Convert.ToDouble(configuration["JwtExpireDays"]) };
                //var data = (email: user.Email, userRole: user.SystemRole.Name, access_token: token, expires: Convert.ToDouble(configuration["JwtExpireDays"]));
                return Ok(data);
            }
            else
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = "INVALID LOGIN ATTEMPT", errors = errors});
            }
        }
    }
}
