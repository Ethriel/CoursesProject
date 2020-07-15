using ServicesAPI.DTO;
using Microsoft.AspNetCore.Http;
using ServicesAPI.Facebook;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IAccountService
    {
        public Task<object> ConfirmEmailAsync(int userId, string token);
        public Task<object> SignInAsync(SystemUserDTO userData);
        public Task<object> SignUpAsync(SystemUserDTO userData, HttpContext httpContext);
        public Task<object> UseFacebookAsync(FacebookUser facebookUser);
    }
}
