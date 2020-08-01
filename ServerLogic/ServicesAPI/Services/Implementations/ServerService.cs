using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServicesAPI.DataPresentation.ErrorHandling;
using ServicesAPI.Services.Abstractions;

namespace ServicesAPI.Services.Implementations
{
    public class ServerService : IServerService
    {
        private readonly ILogger<ServerService> logger;

        public ServerService(ILogger<ServerService> logger)
        {
            this.logger = logger;
        }

        public string GetHangfireHref(HttpContext httpContext)
        {
            var authority = httpContext.Request.Host.Value;

            var hangfire = string.Concat("https://", authority, "/", "hangfire");

            return hangfire;
        }

        public void LogJavascriptError(JavascriptError javascriptError)
        {
            var javascriptException = new JavascriptException(javascriptError.Message, javascriptError.Stack);
            logger.LogError(javascriptException, "Javascript error");
        }
    }
}
