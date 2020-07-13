using Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using ServerAPI.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Services.Abstractions
{
    public interface IAccountService
    {
        public Task<object> ConfirmEmailAsync(int userId, string token);
        public Task<object> SignInAsync(SystemUserDTO userData);
        public Task<object> SignUpAsync(SystemUserDTO userData, HttpContext httpContext);
        public Task<object> UseFacebookAsync(FacebookUser facebookUser);
    }
}
