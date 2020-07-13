using Infrastructure.DbContext;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServerAPI.Facebook;
using ServerAPI.Helpers;
using ServerAPI.MapperWrappers;
using ServerAPI.Responses;
using ServerAPI.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly CoursesSystemDbContext _context;
        private readonly SignInManager<SystemUser> _signInManager;
        private readonly RoleManager<SystemRole> _roleManager;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SecurityTokenHandler _tokenHandler;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> _mapperWrapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;

        public AccountService(CoursesSystemDbContext context, SignInManager<SystemUser> signInManager,
            RoleManager<SystemRole> roleManager,
            UserManager<SystemUser> userManager,
            IConfiguration configuration,
            SecurityTokenHandler tokenHandler,
            IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper,
            IHttpClientFactory httpClientFactory,
            IEmailService emailService)
        {
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenHandler = tokenHandler;
            _mapperWrapper = mapperWrapper;
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;
        }

        public async Task<object> ConfirmEmailAsync(int userId, string token)
        {
            var user = await _context.SystemUsers.FindAsync(userId);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (confirmResult.Succeeded)
            {
                return "Email confirmed";
            }
            else
            {
                var errors = GetIdentityErrors(confirmResult.Errors);
                return errors;
            }
        }
        public async Task<object> SignInAsync(SystemUserDTO userData)
        {
            try
            {
                // find user
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(userData.Email));
                // try to sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, userData.Password, true, false);
                if (signInResult.Succeeded)
                {
                    // if OK - return data
                    var data = GetResponseData(user);
                    return data;
                }
                else
                {
                    // if not - get errors and return
                    var errors = GetSignInErrors(user, signInResult);
                    return errors;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> SignUpAsync(SystemUserDTO userData, HttpContext httpContext)
        {
            try
            {
                var user = _mapperWrapper.MapFromDTO(userData);
                user.UserName = userData.Email;
                user.RegisteredDate = DateTime.Now;
                var role = await _roleManager.FindByNameAsync("USER");
                user.SystemRole = role;
                user.CalculateAge();
                // try to create user
                var creationResult = await _userManager.CreateAsync(user, userData.Password);
                if (creationResult.Succeeded)
                {
                    // if OK - return data
                    user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(userData.Email));
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _emailService.SendConfirmMessageAsync(user.Id, token, user.Email, httpContext.Request.Scheme);
                    var data = GetResponseData(user);
                    return data;
                }
                else
                {
                    // if not - get errors and return
                    var errors = GetIdentityErrors(creationResult.Errors);
                    return errors;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> UseFacebookAsync(FacebookUser facebookUser)
        {
            // an URL to validate facebook access token
            const string VALIDATION_URL = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
            try
            {
                var addId = _configuration["Authentication:Facebook:AppId"];
                var secret = _configuration["Authentication:Facebook:AppSecret"];
                var formattedUrl = string.Format(VALIDATION_URL, facebookUser.AccessToken, addId, secret);

                // send a request to valiedate facebook token
                var validationResult = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
                validationResult.EnsureSuccessStatusCode();
                var responseAsString = await validationResult.Content.ReadAsStringAsync();

                // deserialize response
                var facebookTokenData = JsonConvert.DeserializeObject<FacebookTokenData>(responseAsString);
                var isTokenValid = facebookTokenData.Data.IsValid;

                // if token is not valid - return null
                if (!isTokenValid)
                {
                    return null;
                }
                else
                {
                    // check if user already exists in database
                    var user = await _userManager.FindByEmailAsync(facebookUser.Email);
                    if (user == null)
                    {
                        // if not - create one
                        var role = await _roleManager.FindByNameAsync("USER");
                        var systemUser = new SystemUser
                        {
                            FirstName = facebookUser.FirstName,
                            LastName = facebookUser.LastName,
                            Email = facebookUser.Email,
                            UserName = facebookUser.Email,
                            EmailConfirmed = true,
                            SystemRole = role,
                            AvatarPath = facebookUser.PictureUrl
                        };
                        var creationResult = await _userManager.CreateAsync(systemUser);
                        if (creationResult.Succeeded)
                        {
                            // if creation was successful - return data
                            systemUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(facebookUser.Email));
                            var data = GetResponseData(systemUser);
                            return data;
                        }
                        else
                        {
                            // if not - get errors and return
                            var errors = GetIdentityErrors(creationResult.Errors);
                            return errors;
                        }
                    }
                    else
                    {
                        // if user with such email exists in database - gather account response and return it
                        var data = GetResponseData(user);
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private AccountResponse GetResponseData(SystemUser user)
        {
            var code = Encoding.UTF8.GetBytes(_configuration["JwtKey"]);
            var token = JWTHelper.GenerateJwtToken(user, _configuration, _tokenHandler, code);
            var expire = Convert.ToDouble(_configuration["JwtExpireDays"]);
            var userDTO = _mapperWrapper.MapFromEntity(user);
            var data = new AccountResponse(userDTO, new TokenResponse(token, expire));
            return data;
        }
        private IEnumerable<string> GetIdentityErrors(IEnumerable<IdentityError> errors)
        {
            var creationErrors = errors.Select(x => $"Code: ${x.Code}. Description: {x.Description}");
            return creationErrors;
        }
        private async Task<IEnumerable<string>> GetSignInErrors(SystemUser user, SignInResult signInResult)
        {
            var errors = new List<string>();
            if (signInResult.IsNotAllowed)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    errors.Add("Email is not confirmed");
                }
            }
            else if (signInResult.IsLockedOut)
            {
                errors.Add("User is locked out");
            }
            else if (signInResult.RequiresTwoFactor)
            {
                errors.Add("Two factor authentication is required");
            }
            else
            {
                if (user == null)
                {
                    errors.Add("Username is incorrect");
                }
                else
                {
                    errors.Add("Password is incorrect");
                }
            }
            return errors;
        }


    }
}
