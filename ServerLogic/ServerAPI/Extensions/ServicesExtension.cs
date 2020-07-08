using AutoMapper;
using Infrastructure.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using Infrastructure.DbContext;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServerAPI.Helpers;
using ServerAPI.MapperWrappers;
using ServerAPI.UnitsOfWork;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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

            services.AddAutoMapper(GetAllMapperProfiles.MapperProfiles);

            services.AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>();

            //services.AddScoped<IRepository<SystemUser>, SystemUsersRepository>();

            //services.AddScoped<IRepository<TrainingCourse>, TrainingCoursesRepository>();

            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IMapperWrapper<SystemUser, SystemUserDTO>, SystemUserMapperWrapper>();

            services.AddScoped<IMapperWrapper<TrainingCourse, TrainingCourseDTO>, TrainingCoursesMapperWrapper>();
            services.AddScoped<IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>, SystemUsersTrainingCoursesMapperWrapper>();

            services.AddDbContext<CoursesSystemDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<SystemUser, SystemRole>()
                .AddEntityFrameworkStores<CoursesSystemDbContext>()
                .AddDefaultTokenProviders();

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
                });

            services.AddCors(corsOptions =>
                corsOptions.AddPolicy(configuration["CORS"],
                policyBuilder =>
                policyBuilder.AllowAnyHeader().
                AllowAnyMethod().
                WithOrigins(configuration["client"], configuration["api"]).
                AllowCredentials()));
        }
    }
}
