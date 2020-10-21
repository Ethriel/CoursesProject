using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [Route("[controller]")]
    public class ServerStartedController : Controller
    {
        private readonly IServerService serverService;
        private readonly IAccountService accountService;

        public ServerStartedController(IServerService serverService, IAccountService accountService)
        {
            this.serverService = serverService;
            this.accountService = accountService;
        }

        public async Task<IActionResult> Start()
        {
            ViewBag.Text = "Server is running";
            ViewBag.Href = serverService.GetHangfireHref(HttpContext);
            return View();
        }
    }
}
