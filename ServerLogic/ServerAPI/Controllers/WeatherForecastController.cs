using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAPI.BackgroundJobs;
using ServicesAPI.Services.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IEmailService emailService;
        private readonly RoleManager<SystemRole> roleManager;
        private readonly UserManager<SystemUser> userManager;
        private readonly CoursesSystemDbContext context;
        private readonly IEmailNotifyJob emailNotifyJob;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailService emailService,
            RoleManager<SystemRole> roleManager, UserManager<SystemUser> userManager,
            CoursesSystemDbContext context, IEmailNotifyJob emailNotifyJob)
        {
            _logger = logger;
            this.emailService = emailService;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.context = context;
            this.emailNotifyJob = emailNotifyJob;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();
            return Ok(result);
        }
        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] WeatherForecast forecast)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            else
            {
                return Ok();
            }
        }
    }
}
