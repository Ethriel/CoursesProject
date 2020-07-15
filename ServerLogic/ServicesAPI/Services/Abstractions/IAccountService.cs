using Microsoft.AspNetCore.Http;
using ServicesAPI.DTO;
using ServicesAPI.Facebook;
using ServicesAPI.Responses.AccountResponseData;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{

    public interface IAccountService
    {
        public Task<AccountResponse> ConfirmEmailAsync(int userId, string token);
        public Task<AccountResponse> SignInAsync(SystemUserDTO userData);
        public Task<AccountResponse> SignUpAsync(SystemUserDTO userData, HttpContext httpContext);
        public Task<AccountResponse> UseFacebookAsync(FacebookUser facebookUser);
    }
}
