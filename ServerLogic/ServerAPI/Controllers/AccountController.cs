using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.Helpers;
using ServerAPI.UnitsOfWork;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork usersUnitOfWork;
        private readonly SecurityTokenHandler tokenValidator;
        private readonly IMapper mapper;

        public AccountController(IUnitOfWork usersUnitOfWork,
            IConfiguration configuration,
            SecurityTokenHandler tokenValidator,
            IMapper mapper)
        {
            this.configuration = configuration;
            this.usersUnitOfWork = usersUnitOfWork;
            this.tokenValidator = tokenValidator;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpPost("signup")]
        public async Task<IActionResult> Register(SystemUserDTO userData)
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
            var uow = usersUnitOfWork as SystemUserUnitOfWork;
            var user = MapperHelper<SystemUser, SystemUserDTO>.MapEntityFromDTO(mapper, userData);
            var res = await uow.UserManager.CreateAsync(user, userData.Password);
            if (res.Succeeded)
            {
                var data = await UserResponseHelper.GetResponseData(uow, configuration, tokenValidator, mapper, user);
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
            var uow = usersUnitOfWork as SystemUserUnitOfWork;
            var user = await uow.Users.FindAsync(user => user.Email.Equals(userData.Email));
            user = await uow.Users.GetFullAsync(user.Id);
            var res = await uow.SignInManager.PasswordSignInAsync(user, userData.Password, true, false);
            if (res.Succeeded)
            {
                var data = await UserResponseHelper.GetResponseData(uow, configuration, tokenValidator, mapper, user);
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
