using Microsoft.AspNetCore.Http;
using ServicesAPI.DataPresentation.AccountManagement;
using ServicesAPI.DTO;
using ServicesAPI.Facebook;
using ServicesAPI.Responses;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IAccountService
    {
        public Task<ApiResult> ConfirmEmailAsync(ConfirmEmailData confirmEmailData);
        public Task<ApiResult> ConfirmChangeEmailAsync(ConfirmChangeEmailData confirmChangeEmails);
        public Task<ApiResult> SignInAsync(SystemUserDTO userData);
        public Task<ApiResult> SignUpAsync(SystemUserDTO userData, HttpContext httpContext);
        public Task<ApiResult> SignOutAsync(EmailWrapper emailWrapper);
        public Task<ApiResult> UseFacebookAsync(FacebookUser facebookUser);
        public Task<ApiResult> UpdateAccountAsync(AccountUpdateData accountUpdateData, HttpContext httpContext);
        public Task<ApiResult> VerifyEmailAsync(string email);
        public Task<ApiResult> ResetPasswordAsync(ResetPasswordData resetPasswordData);
        public Task<ApiResult> ForgotPasswordAsync(EmailWrapper emailWrapper);
    }
}
