﻿using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly CoursesSystemDbContext context;
        private readonly SignInManager<SystemUser> signInManager;
        private readonly RoleManager<SystemRole> roleManager;
        private readonly UserManager<SystemUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SecurityTokenHandler tokenHandler;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IEmailService emailService;

        public AccountService(CoursesSystemDbContext context, SignInManager<SystemUser> signInManager,
            RoleManager<SystemRole> roleManager,
            UserManager<SystemUser> userManager,
            IConfiguration configuration,
            SecurityTokenHandler tokenHandler,
            IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper,
            IHttpClientFactory httpClientFactory,
            IEmailService emailService)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.tokenHandler = tokenHandler;
            this.mapperWrapper = mapperWrapper;
            this.httpClientFactory = httpClientFactory;
            this.emailService = emailService;
        }

        public async Task<ApiResult> ConfirmEmailAsync(int userId, string token)
        {
            var result = new ApiResult();

            var user = await context.SystemUsers.FindAsync(userId);

            if (user == null)
            {
                result.SetApiResult(ApiResultStatus.NotFound, $"User with id = {userId} was not found", message: "User not found");
            }
            else
            {
                var confirmResult = await userManager.ConfirmEmailAsync(user, token);

                if (confirmResult.Succeeded)
                {
                    result.SetApiResult(ApiResultStatus.Ok, $"User {user.Email} confirmed email", message: "Email confirmed");
                }
                else
                {
                    var errors = GetIdentityErrors(confirmResult.Errors);
                    result.SetApiResult(ApiResultStatus.BadRequest,
                                        $"User {user.Email} failed to confirm email. Errors: {errors}",
                                        message: "Email confirmation has failed",
                                        errors: errors);
                }
            }

            return result;
        }
        public async Task<ApiResult> SignInAsync(SystemUserDTO userData)
        {
            var result = new ApiResult();

            // find user
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(userData.Email));

            if (user == null)
            {
                var message = $"User with email {user.Email} was not found";
                result.SetApiResult(ApiResultStatus.NotFound, message, message: message);
            }
            else
            {
                // try to sign in
                var signInResult = await signInManager.PasswordSignInAsync(user, userData.Password, true, false);

                if (signInResult.Succeeded)
                {
                    // if OK - get user data and return OK
                    var data = GetAccountData(user);
                    result.SetApiResult(ApiResultStatus.Ok, $"User email {user.Email} has signed in", data);

                }
                else
                {
                    // if not - set message, set errors and return Bad Request
                    var errors = new string[] { "Password is incorrect" };
                    result.SetApiResult(ApiResultStatus.BadRequest,
                                        $"User email {user.Email} has failed to sign in with password",
                                        "Sign in failed",
                                        errors: errors);
                }
            }

            return result;
        }

        public async Task<ApiResult> SignUpAsync(SystemUserDTO userData, HttpContext httpContext)
        {
            var result = new ApiResult();

            var findUser = await userManager.FindByEmailAsync(userData.Email);

            if (findUser != null)
            {
                var errors = new string[] { $"User with {userData.Email} already exists in the database" };
                result.SetApiResult(ApiResultStatus.BadRequest,
                                    $"New user {userData.Email} has tried to sign up. User with this email already exists in the database",
                                    message: "Sign up has failed",
                                    errors: errors);
            }
            else
            {
                var user = await MapNewUserFromDTO(userData);

                // try to create user
                result = await TryCreateUser(user, userData.Password, result, httpContext);
            }

            return result;
        }

        public async Task<SystemUser> MapNewUserFromDTO(SystemUserDTO userData)
        {
            var user = mapperWrapper.MapFromDTO(userData);

            user.UserName = userData.Email;
            user.RegisteredDate = DateTime.Now;

            var role = await roleManager.FindByNameAsync("USER");
            user.SystemRole = role;
            user.CalculateAge();

            return user;
        }

        private async Task<ApiResult> TryCreateUser(SystemUser user, string password, ApiResult result, HttpContext httpContext)
        {
            var creationResult = await userManager.CreateAsync(user, password);

            if (creationResult.Succeeded)
            {
                // client side needs to know about user's Id, so we re-assign user with one from database
                user = await userManager.FindByEmailAsync(user.Email);

                // send confirm request on user's email
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                await emailService.SendConfirmMessageAsync(user.Id, token, user.Email, httpContext.Request.Scheme);

                var data = GetAccountData(user);
                result.SetApiResult(ApiResultStatus.Ok, $"User {user.Email} signed up", data);
            }
            else
            {
                // if not - set message, set errors and return Bad Request
                var errors = GetIdentityErrors(creationResult.Errors);
                result.SetApiResult(ApiResultStatus.BadRequest,
                                    $"New user {user.Email} has failed to sign up. Errors: {errors}",
                                    message: "Sign up failed",
                                    errors: errors);
            }

            return result;
        }

        public async Task<ApiResult> UseFacebookAsync(FacebookUser facebookUser)
        {
            var result = new ApiResult();

            var isTokenValid = await CheckFaceBookAccessToken(facebookUser);

            // if token is not valid - set message
            if (!isTokenValid)
            {
                result.SetApiResult(ApiResultStatus.BadRequest,
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
                    result = await TryCreateSystemUserFromFacebook(facebookUser, result);
                }
                else
                {
                    // if user with such email exists in database - gather account data and return OK
                    var data = GetAccountData(user);
                    result.SetApiResult(ApiResultStatus.Ok,
                                        $"User {facebookUser.Email} has successfully signed in with Facebook",
                                        data);
                }
            }

            return result;
        }
        private async Task<bool> CheckFaceBookAccessToken(FacebookUser facebookUser)
        {
            // an URL to validate facebook access token
            const string FACEBOOK_VALIDATION_URL = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";

            var addId = configuration["Authentication:Facebook:AppId"];
            var secret = configuration["Authentication:Facebook:AppSecret"];
            var formattedUrl = string.Format(FACEBOOK_VALIDATION_URL, facebookUser.AccessToken, addId, secret);

            // send a request to valiedate facebook token
            var validationResult = await httpClientFactory.CreateClient().GetAsync(formattedUrl);
            validationResult.EnsureSuccessStatusCode();
            var responseAsString = await validationResult.Content.ReadAsStringAsync();

            // deserialize response
            var facebookTokenData = JsonConvert.DeserializeObject<FacebookTokenData>(responseAsString);
            var isTokenValid = facebookTokenData.Data.IsValid;

            return isTokenValid;
        }
        private async Task<ApiResult> TryCreateSystemUserFromFacebook(FacebookUser facebookUser, ApiResult result)
        {
            var systemUser = await GetSystemUserFromFacebook(facebookUser);

            var creationResult = await userManager.CreateAsync(systemUser);

            if (creationResult.Succeeded)
            {
                // we need Id of new user to be sent to the client, so we re-assign user
                systemUser = await userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(facebookUser.Email));

                // if creation was successful - set data and OK
                var data = GetAccountData(systemUser);
                result.SetApiResult(ApiResultStatus.Ok, $"User {systemUser.Email} has signed up with Facebook", data);
            }
            else
            {
                // if not - get errors and return Bad Request
                var errors = GetIdentityErrors(creationResult.Errors);
                result.SetApiResult(ApiResultStatus.BadRequest,
                                    $"Creation of user {systemUser.Email} has failed after using Facebook. Errors: {errors}",
                                    message: "Sign up with Facebook has failed",
                                    errors: errors);
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
        private AccountData GetAccountData(SystemUser user)
        {
            var code = Encoding.UTF8.GetBytes(configuration["JwtKey"]);
            var token = JWTHelper.GenerateJwtToken(user, configuration, tokenHandler, code);
            var expire = Convert.ToDouble(configuration["JwtExpireDays"]);
            var userDTO = mapperWrapper.MapFromEntity(user);
            var data = new AccountData(userDTO, new TokenData(token, expire));
            return data;
        }
        private IEnumerable<string> GetIdentityErrors(IEnumerable<IdentityError> errorsCollection)
        {
            var errors = errorsCollection.Select(x => x.Description);
            return errors;
        }
    }
}
