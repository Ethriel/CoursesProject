using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("[controller]")]
    public class ServerStartedController : Controller
    {
        public IActionResult Start()
        {
            var authority = HttpContext.Request.Host.Value;

            var hangfire = string.Concat("https://", authority, "/", "hangfire");
            ViewBag.Text = "Server has started";
            ViewBag.Href = hangfire;
            return View();
        }
    }
}
