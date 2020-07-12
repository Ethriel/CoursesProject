﻿using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.MapperWrappers;
using System;
using System.Text;

namespace ServerAPI.Helpers
{
    public class UserResponseHelper
    {
        public static object GetResponseData(IConfiguration configuration, SecurityTokenHandler tokenValidator, IMapperWrapper<SystemUser, SystemUserDTO> wrapper, SystemUser user, string accessToken = null)
        {
            var code = Encoding.UTF8.GetBytes(configuration["JwtKey"]);
            var token = accessToken ?? JWTHelper.GenerateJwtToken(user, configuration, tokenValidator, code);
            var expire = Convert.ToDouble(configuration["JwtExpireDays"]);
            var userDTO = wrapper.MapFromEntity(user);
            var data = new { user = userDTO, token = new { key = token, expires = expire } };
            return data;
        }
    }
}
