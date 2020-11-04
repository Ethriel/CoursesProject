using ServicesAPI.DataPresentation.ErrorHandling;

namespace ServicesAPI.Services.Abstractions
{
    public interface IServerService
    {
        string GetHangfireHref();
        string GetServerURL();
        string GetRootPath(string folder);
        void LogJavascriptError(JavascriptError javascriptError);
    }
}
