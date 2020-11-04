using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServicesAPI.DataPresentation.ErrorHandling;
using ServicesAPI.Services.Abstractions;
using System.IO;

namespace ServicesAPI.Services.Implementations
{
    public class ServerService : IServerService
    {
        private readonly ILogger<ServerService> logger;
        private readonly IWebHostEnvironment webHost;
        private readonly HttpContext httpContext;

        public ServerService(ILogger<ServerService> logger, IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.webHost = webHost;
            httpContext = httpContextAccessor.HttpContext;
        }

        public string GetHangfireHref()
        {
            var authority = GetAuthority();

            var url = GetURL(authority);

            var hangfire = string.Concat(url, "hangfire");

            return hangfire;
        }

        public string GetRootPath(string folder)
        {
            var root = Path.Combine(webHost.WebRootPath, "share", "img", folder);
            return root;
        }

        public string GetServerURL()
        {
            var authority = GetAuthority();

            var url = GetURL(authority);

            return url;
        }

        public void LogJavascriptError(JavascriptError javascriptError)
        {
            var javascriptException = new JavascriptException(javascriptError.Message, javascriptError.Stack);
            logger.LogError(javascriptException, "Javascript error");
        }

        private string GetAuthority()
        {
            return httpContext.Request.Host.Value;
        }

        private string GetURL(string authority)
        {
            return string.Concat("https://", authority, "/");
        }
    }
}
