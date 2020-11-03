using Microsoft.AspNetCore.Http;
using ServicesAPI.DataPresentation.ErrorHandling;

namespace ServicesAPI.Services.Abstractions
{
    public interface IServerService
    {
        string GetHangfireHref(HttpContext httpContext);
        string GetServerURL(HttpContext httpContext);
        string GetRootPath(string folder);
        void LogJavascriptError(JavascriptError javascriptError);
    }
}
