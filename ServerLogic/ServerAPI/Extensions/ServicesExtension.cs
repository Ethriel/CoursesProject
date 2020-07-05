using AutoMapper;
using Infrastructure.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServerAPI.Helpers;
using ServerAPI.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Extensions
{
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(GetAllMapperProfiles.MapperProfiles);

            services.AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>();

            services.AddTransient<IUnitOfWork, SystemUserUnitOfWork>();

            services.AddTransient<IRepository<SystemUser>, SystemUsersRepository>();

            services.AddTransient<IRepository<TrainingCourse>, TrainingCoursesRepository>();
        }
    }
}
