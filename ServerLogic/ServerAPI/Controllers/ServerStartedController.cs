using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Services.Abstractions;

namespace ServerAPI.Controllers
{
    [Route("[controller]")]
    public class ServerStartedController : Controller
    {
        private readonly IServerService serverService;

        public ServerStartedController(IServerService serverService)
        {
            this.serverService = serverService;
        }

        public IActionResult Start()
        {
            ViewBag.Text = "Server is running";
            ViewBag.Href = serverService.GetHangfireHref(HttpContext);
            return View();
        }
    }
}
