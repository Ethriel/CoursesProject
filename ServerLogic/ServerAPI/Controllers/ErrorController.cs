using Microsoft.AspNetCore.Mvc;
using ServicesAPI.DataPresentation.ErrorHandling;
using ServicesAPI.Services.Abstractions;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : Controller
    {
        private readonly IServerService serverService;
        public ErrorController(IServerService serverService)
        {
            this.serverService = serverService;
        }

        [HttpPost("logJavascriptError")]
        public IActionResult LogJavascriptError([FromBody] JavascriptError javascriptError)
        {
            serverService.LogJavascriptError(javascriptError);
            return Ok();
        }
    }
}
