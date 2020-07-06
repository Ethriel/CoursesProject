using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.Helpers;
using ServerAPI.MapperWrappers;
using ServerAPI.UnitsOfWork;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly SecurityTokenHandler tokenValidator;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;

        public AccountController(IUnitOfWork unitOfWork,
            IConfiguration configuration,
            SecurityTokenHandler tokenValidator,
            IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper)
        {
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.tokenValidator = tokenValidator;
            this.mapperWrapper = mapperWrapper;
        }

        [Authorize]
        [HttpPost("signup")]
        public async Task<IActionResult> Register(SystemUserDTO userData)
        {
            var user = mapperWrapper.MapFromDTO(userData);
            var res = await unitOfWork.UserManager.CreateAsync(user, userData.Password);
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
            var user = await unitOfWork.Users.FindAsync(user => user.Email.Equals(userData.Email));
            user = await unitOfWork.Users.GetFullAsync(user.Id);
            var res = await unitOfWork.SignInManager.PasswordSignInAsync(user, userData.Password, true, false);
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
