using AutoMapper;
using Infrastructure.DbContext;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.Helpers;
using System;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AccountController : Controller
    {
        private readonly CoursesSystemDbContext context;
        private readonly UserManager<SystemUser> userManager;
        private readonly SignInManager<SystemUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly SecurityTokenHandler tokenValidator;
        private readonly IMapper mapper;

        public AccountController(CoursesSystemDbContext context,
            UserManager<SystemUser> userManager,
            SignInManager<SystemUser> signInManager,
            IConfiguration configuration,
            SecurityTokenHandler tokenValidator,
            IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.tokenValidator = tokenValidator;
            this.mapper = mapper;
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
                return BadRequest(new { message = "INVALID LOGIN ATTEMPT", errors = errors });
            }
        }
    }
}
