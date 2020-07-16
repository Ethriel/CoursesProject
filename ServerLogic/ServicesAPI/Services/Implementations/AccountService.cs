using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServicesAPI.DTO;
using ServicesAPI.ErrorHandle.ApiExceptions;
using ServicesAPI.Facebook;
using ServicesAPI.Helpers;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses.AccountResponseData;
using ServicesAPI.Services.Abstractions;
using System;
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

        public async Task<AccountResponse> ConfirmEmailAsync(int userId, string token)
        {
            AccountResponse response = null;
            var user = await context.SystemUsers.FindAsync(userId);


            var confirmResult = await userManager.ConfirmEmailAsync(user, token);

            if (confirmResult.Succeeded)
            {
                response = new AccountResponse(null, AccountOperationResult.Succeeded);
            }
            else
            {
                response = new AccountResponse(null, AccountOperationResult.Failed);
            }
            return response;

        }
        public async Task<AccountResponse> SignInAsync(SystemUserDTO userData)
        {
            AccountResponse response = null;
            // find user
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(userData.Email));


            // try to sign in
            var signInResult = await signInManager.PasswordSignInAsync(user, userData.Password, true, false);

            if (signInResult.Succeeded)
            {
                // if OK - set data and succeeded
                var data = GetAccountData(user);
                response = new AccountResponse(data, AccountOperationResult.Succeeded);

            }
            else
            {
                // if not - set failed and no data
                response = new AccountResponse(null, AccountOperationResult.Failed);
            }
            return response;
        }

        public async Task<AccountResponse> SignUpAsync(SystemUserDTO userData, HttpContext httpContext)
        {
            AccountResponse response = null;
            var user = mapperWrapper.MapFromDTO(userData);
            user.UserName = userData.Email;
            user.RegisteredDate = DateTime.Now;
            var role = await roleManager.FindByNameAsync("USER");
            user.SystemRole = role;
            user.CalculateAge();

            // try to create user
            var creationResult = await userManager.CreateAsync(user, userData.Password);

            if (creationResult.Succeeded)
            {
                // if OK - set data and succeeded
                user = await userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(userData.Email));

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                await emailService.SendConfirmMessageAsync(user.Id, token, user.Email, httpContext.Request.Scheme);

                var data = GetAccountData(user);

                response = new AccountResponse(data, AccountOperationResult.Succeeded);
            }
            else
            {
                // if not - set failed and no data
                response = new AccountResponse(null, AccountOperationResult.Failed);
            }

            return response;
        }

        public async Task<AccountResponse> UseFacebookAsync(FacebookUser facebookUser)
        {
            // an URL to validate facebook access token
            const string FACEBOOK_VALIDATION_URL = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";

            AccountResponse response = null;

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

            // if token is not valid - set failed and no data
            if (!isTokenValid)
            {
                response = new AccountResponse(null, AccountOperationResult.Failed);
            }
            else
            {
                var user = await userManager.FindByEmailAsync(facebookUser.Email);

                // check if user already exists in database
                if (user == null)
                {
                    // if not - create one
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

                    var creationResult = await userManager.CreateAsync(systemUser);
                    if (creationResult.Succeeded)
                    {
                        // if creation was successful - set data and succeeded
                        systemUser = await userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(facebookUser.Email));

                        var data = GetAccountData(systemUser);

                        response = new AccountResponse(data, AccountOperationResult.Succeeded);
                    }
                    else
                    {
                        // if not - get errors and return
                        response = new AccountResponse(null, AccountOperationResult.Failed);
                    }
                }
                else
                {
                    // if user with such email exists in database - gather account data and set succeeded
                    var data = GetAccountData(user);

                    response = new AccountResponse(data, AccountOperationResult.Succeeded);
                }
            }

            return response;

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
    }
}
