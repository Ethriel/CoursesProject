using ServicesAPI.DTO;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServicesAPI.MapperWrappers;
using System;
using System.Text;

namespace ServicesAPI.Helpers
{
    public class UserResponseHelper
    {
        public static object GetResponseData(IConfiguration configuration, SecurityTokenHandler tokenValidator, IMapperWrapper<SystemUser, SystemUserDTO> wrapper, SystemUser user)
        {
            var code = Encoding.UTF8.GetBytes(configuration["JwtKey"]);
            var token = JWTHelper.GenerateJwtToken(user, configuration, tokenValidator, code);
            var expire = Convert.ToDouble(configuration["JwtExpireDays"]);
            var userDTO = wrapper.MapFromEntity(user);
            var data = new { user = userDTO, token = new { key = token, expires = expire } };
            return data;
        }
    }
}
