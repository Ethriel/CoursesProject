using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.UnitsOfWork;
using System;
using System.Threading.Tasks;

namespace ServerAPI.Helpers
{
    public class UserResponseHelper
    {
        public async static Task<object> GetResponseData(SystemUserUnitOfWork unitOfWork, IConfiguration configuration, SecurityTokenHandler tokenValidator, IMapper mapper, SystemUser user)
        {
            var code = await unitOfWork.UserManager.CreateSecurityTokenAsync(user);
            var token = JWTHelper.GenerateJwtToken(user, configuration, tokenValidator, code);
            var expire = Convert.ToDouble(configuration["JwtExpireDays"]);
            var userDTO = MapperHelper<SystemUser, SystemUserDTO>.MapDTOFromEntity(mapper, user);
            var data = new { user = userDTO, token = new { key = token, expires = expire } };
            return data;
        }
    }
}
