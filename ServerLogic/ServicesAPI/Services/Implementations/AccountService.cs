using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServicesAPI.DataPresentation.AccountManagement;
using ServicesAPI.DTO;
using ServicesAPI.Facebook;
using ServicesAPI.Helpers;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Responses.AccountResponseData;
using ServicesAPI.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<SystemUser> signInManager;
        private readonly RoleManager<SystemRole> roleManager;
        private readonly UserManager<SystemUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SecurityTokenHandler tokenHandler;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IEmailService emailService;
        private readonly IExtendedDataService<SystemUser> users;
        private readonly IImageWorker imageWorker;

        public AccountService(SignInManager<SystemUser> signInManager,
            RoleManager<SystemRole> roleManager,
            UserManager<SystemUser> userManager,
            IConfiguration configuration,
            SecurityTokenHandler tokenHandler,
            IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper,
            IHttpClientFactory httpClientFactory,
            IEmailService emailService,
            IExtendedDataService<SystemUser> users,
            IImageWorker imageWorker)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.tokenHandler = tokenHandler;
            this.mapperWrapper = mapperWrapper;
            this.httpClientFactory = httpClientFactory;
            this.emailService = emailService;
            this.users = users;
            this.imageWorker = imageWorker;
        }

        public async Task<ApiResult> ConfirmEmailAsync(ConfirmEmailData confirmEmailData)
        {
            var result = default(ApiResult);
            var userId = confirmEmailData.Id;
            var user = await users.GetByIdAsync(userId);

            if (user == null)
            {
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, $"User with id = {userId} was not found", message: "User not found");
            }
            else
            {
                result = await GetConfirmEmailResultAsync(user, confirmEmailData.Token);
            }

            return result;
        }
        public async Task<ApiResult> CheckEmailConfirmedAsync(EmailWrapper emailWrapper)
        {
            var result = default(ApiResult);
            var email = emailWrapper.Email;
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var message = "User not found";
                var loggerMessage = $"User {email} not found";
                var errors = new string[] { loggerMessage };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                if (user.EmailConfirmed)
                {
                    result = ApiResult.GetOkResult(ApiResultStatus.Ok);
                }
                else
                {
                    var message = "Email is not confirmed";
                    var loggerMessage = $"Email { email} is not confirmed. But you still can browse courses";
                    var errors = new string[] { loggerMessage };
                    result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
                }
            }

            return result;
        }

        public async Task<ApiResult> ConfirmEmailRequestAsync(EmailWrapper emailWrapper)
        {
            var result = default(ApiResult);
            var email = emailWrapper.Email;
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var message = "User not found";
                var loggerMessage = $"User {email} not found";
                var errors = new string[] { loggerMessage };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                await SendEmailConfirmAsync(user);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, message: "A confirm message was sent to your email. Follow the instructions");
            }

            return result;
        }
        public async Task<ApiResult> ConfirmChangeEmailAsync(ConfirmChangeEmailData confirmChangeEmail)
        {
            var result = default(ApiResult);
            var userId = confirmChangeEmail.Id;
            var user = await users.GetByIdAsync(userId);

            if (user == null)
            {
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, $"User with id = {userId} was not found", message: "User not found");
            }
            else
            {
                result = await GetChangeEmailResultAsync(user, confirmChangeEmail.Email, confirmChangeEmail.Token);
            }

            return result;
        }

        public async Task<ApiResult> SignInAsync(SystemUserDTO userData)
        {
            var result = default(ApiResult);
            // find user
            var user = await userManager.FindByEmailAsync(userData.Email);

            if (user == null)
            {
                var message = "Sign in has failed";
                var loggerMessage = $"Sign in has failed for {userData.Email}";
                var errors = new string[] { $"Email {userData.Email} is incorrect" };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                // try to sign in
                var signInResult = await signInManager.PasswordSignInAsync(user, userData.Password, true, false);

                if (signInResult.Succeeded)
                {
                    // if OK - get user data and return OK
                    var data = await GetAccountData(user);
                    result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

                }
                else
                {
                    // if not - set message, set errors and return Bad Request
                    var message = "Sign in has failed";
                    var errors = new string[] { "Password is incorrect" };
                    result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest,
                                        $"User email {user.Email} has failed to sign in with password",
                                        message: message,
                                        errors: errors);
                }
            }

            return result;
        }

        public async Task<ApiResult> SignUpAsync(SystemUserDTO userData)
        {
            var result = default(ApiResult);
            var findUser = await userManager.FindByEmailAsync(userData.Email);

            if (findUser != null)
            {
                var message = "Sign up has failed";
                var loggerMessage = $"New user {userData.Email} has tried to sign up. User with this email already exists in the database";
                var errors = new string[] { $"User {userData.Email} is already registered. Try to sign in" };
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }
            else
            {
                var user = await MapNewUserFromDTO(userData);

                // try to create user
                result = await TryCreateUser(user, userData.Password);
            }

            return result;
        }
        public async Task<ApiResult> SignOutAsync(EmailWrapper emailWrapper)
        {
            var result = default(ApiResult);
            var email = emailWrapper.Email;
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var message = "User not found";
                var loggerMessage = $"User {email} not found";
                var errors = new string[] { };

                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                var stampResult = await userManager.UpdateSecurityStampAsync(user);

                if (stampResult.Succeeded)
                {
                    await signInManager.SignOutAsync();
                    result = ApiResult.GetOkResult(ApiResultStatus.Ok);
                }
                else
                {
                    var message = "Sign out error";
                    var loggerMessage = $"{message} for user {email}";
                    var errors = GetIdentityErrors(stampResult.Errors);
                    result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
                }

            }
            return result;
        }

        public async Task<ApiResult> UseFacebookAsync(FacebookUser facebookUser)
        {
            var result = default(ApiResult);
            var isTokenValid = await CheckFacebookAccessToken(facebookUser);

            // if token is not valid - set message
            if (!isTokenValid)
            {
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest,
                                    $"Invalid Facebook token for user {facebookUser.Email}",
                                    message: "Facebook access token is invalid. Please, try again");
            }
            else
            {
                // if valid - try to register user in the system
                // check if user already exists in database
                var user = await userManager.FindByEmailAsync(facebookUser.Email);

                if (user == null)
                {
                    // if not - try create one
                    result = await TryCreateSystemUserFromFacebook(facebookUser);
                }
                else
                {
                    // if user with such email exists in database - gather account data and return OK
                    var data = await GetAccountData(user);
                    result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);
                }
            }

            return result;
        }
        public async Task<ApiResult> UpdateAccountAsync(AccountUpdateData accountUpdateData)
        {
            var result = default(ApiResult);
            var id = accountUpdateData.User.Id;
            var user = await users.GetByIdAsync(id);

            if (user == null)
            {
                var message = "User was not found";
                var loggerMessage = $"User id = {id} was not found";
                var errors = new string[] { message };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                if (accountUpdateData.IsEmailChanged)
                {
                    await SendConfirmUpdateEmailMessageAsync(user, accountUpdateData.User.Email);
                }

                if (accountUpdateData.AnyFieldChanged)
                {
                    var newUser = mapperWrapper.MapEntity(accountUpdateData.User);

                    //user = UpdateHelper<SystemUser>.Update(context.Model, user, newUser);

                    user = await users.UpdateAsync(user, newUser);

                    //await context.SaveChangesAsync();
                }

                var data = await GetAccountData(user);

                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);
            }

            return result;
        }
        public async Task<ApiResult> VerifyEmailAsync(string email)
        {
            var result = default(ApiResult);
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var message = "Email change has failed";
                var loggerMessage = $"User {email} is already registered";
                var errors = new string[] { loggerMessage };
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }
            else
            {
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: new { varified = true });
            }

            return result;
        }
        public async Task<ApiResult> ResetPasswordAsync(ResetPasswordData resetPasswordData)
        {
            var result = default(ApiResult);
            var email = resetPasswordData.Email;
            var token = resetPasswordData.Token;
            var password = resetPasswordData.Password;

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var message = "User not found";
                var loggerMessage = $"User {email} not found";
                var errors = new string[] { };

                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                var resetResult = await userManager.ResetPasswordAsync(user, token, password);

                if (resetResult.Succeeded)
                {
                    var message = "Password was reseted. Use your new password to sign in";

                    result = ApiResult.GetOkResult(ApiResultStatus.Ok, message: message);
                }
                else
                {
                    var message = "Password reset error";
                    var loggerMessage = $"{message} for {email}";
                    var errors = GetIdentityErrors(resetResult.Errors);

                    result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
                }
            }

            return result;
        }

        public async Task<ApiResult> ForgotPasswordAsync(EmailWrapper emailWrapper)
        {
            var result = default(ApiResult);
            var email = emailWrapper.Email;
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var message = "User not found";
                var loggerMessage = $"User {email} not found";
                var errors = new string[] { loggerMessage };

                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                token = token.Replace("+", "%2B");

                await emailService.SendResetPasswordData(token, email);

                var message = "A confirm token was sent to your email. Follow the instructions";

                result = ApiResult.GetOkResult(ApiResultStatus.Ok, message: message);
            }

            return result;
        }
        private async Task SendConfirmUpdateEmailMessageAsync(SystemUser user, string email)
        {
            var token = await userManager.GenerateChangeEmailTokenAsync(user, email);
            token = token.Replace("+", "%2B");
            await emailService.SendConfirmChangeEmailAsync(token, email);
        }
        private async Task<SystemUser> MapNewUserFromDTO(SystemUserDTO userData)
        {
            var user = mapperWrapper.MapEntity(userData);
            var role = await roleManager.FindByNameAsync("USER");

            user.UserName = userData.Email;
            user.RegisteredDate = DateTime.Now;
            user.SystemRole = role;
            user.CalculateAge();

            return user;
        }
        private async Task<ApiResult> TryCreateUser(SystemUser user, string password)
        {
            var result = default(ApiResult);
            var creationResult = await userManager.CreateAsync(user, password);

            if (creationResult.Succeeded)
            {
                // client side needs to know about user's Id, so we re-assign user with one from database
                user = await userManager.FindByEmailAsync(user.Email);

                // send confirm request on user's email
                await SendEmailConfirmAsync(user);

                var data = await GetAccountData(user);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);
            }
            else
            {
                // if not - set message, set errors and return Bad Request
                var message = "Sign up failed";
                var errors = GetIdentityErrors(creationResult.Errors);
                var loggerMessage = $"New user {user.Email} has failed to sign up. Errors: {errors}";
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }

            return result;
        }
        private async Task SendEmailConfirmAsync(SystemUser user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = token.Replace("+", "%2B");
            await emailService.SendConfirmMessageAsync(token, user.Email);
        }
        private async Task<ApiResult> GetConfirmEmailResultAsync(SystemUser user, string token)
        {
            var result = default(ApiResult);
            var confirmResult = await userManager.ConfirmEmailAsync(user, token);

            if (confirmResult.Succeeded)
            {
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, message: "Email confirmed");
            }
            else
            {
                var message = "Email confirmation has failed";
                var errors = GetIdentityErrors(confirmResult.Errors);
                var loggerMessage = $"User {user.Email} failed to confirm email. Errors: {errors}";
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }

            return result;
        }
        private async Task<ApiResult> GetChangeEmailResultAsync(SystemUser user, string email, string token)
        {
            var result = default(ApiResult);
            var confirmResult = await userManager.ChangeEmailAsync(user, email, token);

            if (confirmResult.Succeeded)
            {
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, message: "Email was changed");
            }
            else
            {
                var message = "Email change has failed";
                var errors = GetIdentityErrors(confirmResult.Errors);
                var loggerMessage = $"User {user.Email} has failed to change email. Errors: {errors}";
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }

            return result;
        }
        private async Task<bool> CheckFacebookAccessToken(FacebookUser facebookUser)
        {
            // an URL to validate facebook access token
            var facebookBaseURL = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";

            var addId = configuration["Authentication:Facebook:AppId"];
            var secret = configuration["Authentication:Facebook:AppSecret"];
            var formattedUrl = string.Format(facebookBaseURL, facebookUser.AccessToken, addId, secret);

            // send a request to valiedate facebook token
            var validationResult = await httpClientFactory.CreateClient()
                                                          .GetAsync(formattedUrl);
            validationResult.EnsureSuccessStatusCode();
            var responseAsString = await validationResult.Content
                                                         .ReadAsStringAsync();

            // deserialize response
            var facebookTokenData = JsonConvert.DeserializeObject<FacebookTokenData>(responseAsString);
            var isTokenValid = facebookTokenData.Data.IsValid;

            return isTokenValid;
        }
        private async Task<ApiResult> TryCreateSystemUserFromFacebook(FacebookUser facebookUser)
        {
            var result = default(ApiResult);
            var systemUser = await GetSystemUserFromFacebook(facebookUser);
            var creationResult = await userManager.CreateAsync(systemUser);

            if (creationResult.Succeeded)
            {
                // we need Id of new user to be sent to the client, so we re-assign user
                systemUser = await userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(facebookUser.Email));

                // if creation was successful - set data and OK
                var data = await GetAccountData(systemUser);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);
            }
            else
            {
                // if not - get errors and return Bad Request
                var message = "Sign up with Facebook has failed";
                var errors = GetIdentityErrors(creationResult.Errors);
                var loggerMessage = $"Creation of user {systemUser.Email} has failed after using Facebook. Errors: {errors}";
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }

            return result;
        }
        private async Task<SystemUser> GetSystemUserFromFacebook(FacebookUser facebookUser)
        {
            var role = await roleManager.FindByNameAsync("USER");

            var systemUser = new SystemUser
            {
                FirstName = facebookUser.FirstName,
                LastName = facebookUser.LastName,
                Email = facebookUser.Email,
                UserName = facebookUser.Email,
                EmailConfirmed = true,
                SystemRole = role,
                AvatarPath = facebookUser.PictureUrl,
                RegisteredDate = DateTime.Now
            };

            systemUser.CalculateAge();

            return systemUser;
        }
        private async Task<AccountData> GetAccountData(SystemUser user)
        {
            var code = Encoding.UTF8.GetBytes(configuration["JwtKey"]);
            var roles = await userManager.GetRolesAsync(user);
            var token = JWTHelper.GenerateJwtToken(user, configuration, tokenHandler, code, roles);
            var expire = Convert.ToDouble(configuration["JwtExpireDays"]);
            var userDTO = mapperWrapper.MapModel(user);
            var avatarURL = imageWorker.GetImageURL("users", user.AvatarPath);
            userDTO.AvatarPath = avatarURL;
            var data = new AccountData(userDTO, new TokenData(token, expire));
            return data;
        }
        private IEnumerable<string> GetIdentityErrors(IEnumerable<IdentityError> errorsCollection)
        {
            var errors = errorsCollection.Select(x => x.Description);
            return errors;
        }

        public async Task AddRoles()
        {
            var user = new SystemRole
            {
                Name = "USER",
                NormalizedName = "User"
            };

            var admin = new SystemRole
            {
                Name = "ADMIN",
                NormalizedName = "Admin"
            };

            var resUser = await roleManager.CreateAsync(user);
            var adminUser = await roleManager.CreateAsync(admin);
        }
    }
}
