using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServicesAPI.BackgroundJobs;
using ServicesAPI.DTO;
using ServicesAPI.Helpers;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Services.Abstractions;
using ServicesAPI.Services.Implementations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace ServerAPI.Extensions
{
    public static class ServicesExtension
    {
        /// <summary>
        /// An extension method for IServiceCollection <paramref name="services"/> that adds all necessary services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            AddHangfireService(services, configuration);

            AddCustomServices(services);

            services.AddAutoMapper(GetAllMapperProfiles.MapperProfiles);

            services.AddHttpClient();

            services.AddDbContext<CoursesSystemDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<SystemUser, SystemRole>()
                    .AddEntityFrameworkStores<CoursesSystemDbContext>()
                    .AddDefaultTokenProviders();

            AddAuthenticationServices(services, configuration);

            AddCorsServices(services, configuration);
        }

        private static void AddHangfireService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                     .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                     .UseSimpleAssemblyNameTypeSerializer()
                     .UseRecommendedSerializerSettings()
                     .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"),
                     new SqlServerStorageOptions
                     {
                         CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                         SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                         QueuePollInterval = TimeSpan.Zero,
                         UseRecommendedIsolationLevel = true,
                         DisableGlobalLocks = true
                     }));

            services.AddHangfireServer();
        }
        private static void AddCustomServices(IServiceCollection services)
        {
            services.AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>();

            AddMapperWrapperServices(services);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            AddServicesForControllers(services);
        }
        private static void AddServicesForControllers(IServiceCollection services)
        {
            services.AddScoped<ISendEmailService, SendEmailService>();

            services.AddScoped<IEmailNotifyJob, EmailNotifyJob>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ICoursesService, CoursesService>();

            services.AddScoped<IStudentsService, StudentsService>();

            services.AddScoped<IUserCoursesService, UserCoursesService>();

        }
        private static void AddMapperWrapperServices(IServiceCollection services)
        {
            services.AddScoped<IMapperWrapper<SystemUser, SystemUserDTO>, SystemUserMapperWrapper>();

            services.AddScoped<IMapperWrapper<TrainingCourse, TrainingCourseDTO>, TrainingCoursesMapperWrapper>();

            services.AddScoped<IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>, SystemUsersTrainingCoursesMapperWrapper>();
        }
        private static void AddAuthenticationServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(configBearer =>
                {
                    configBearer.RequireHttpsMetadata = false;
                    configBearer.SaveToken = true;

                    configBearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = (context) =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode.Equals(200))
                        {
                            context.Response.StatusCode = 401;
                        }

                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = (context) =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
                        {
                            context.Response.StatusCode = 403;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }
        private static void AddCorsServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(corsOptions =>
                corsOptions.AddPolicy(configuration["CORS"],
                policyBuilder =>
                policyBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins(configuration["client"], configuration["api"])
                .AllowCredentials()));
        }
    }
}
