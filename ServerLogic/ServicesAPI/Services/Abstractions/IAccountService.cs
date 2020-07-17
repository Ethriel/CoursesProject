using Microsoft.AspNetCore.Http;
using ServicesAPI.DTO;
using ServicesAPI.Facebook;
using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{

    public interface IAccountService
    {
        public Task<ApiResult> ConfirmEmailAsync(int userId, string token);
        public Task<ApiResult> SignInAsync(SystemUserDTO userData);
        public Task<ApiResult> SignUpAsync(SystemUserDTO userData, HttpContext httpContext);
        public Task<ApiResult> UseFacebookAsync(FacebookUser facebookUser);
    }
}
