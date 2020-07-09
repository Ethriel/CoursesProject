using Infrastructure.DbContext;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.Helpers;
using ServerAPI.MapperWrappers;
using System;
using System.Threading.Tasks;

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

        public AccountController(CoursesSystemDbContext context, SignInManager<SystemUser> signInManager,
            RoleManager<SystemRole> roleManager,
            UserManager<SystemUser> userManager,
            IConfiguration configuration,
            SecurityTokenHandler tokenValidator,
            IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.tokenValidator = tokenValidator;
            this.mapperWrapper = mapperWrapper;
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
    }
}
