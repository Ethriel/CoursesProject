﻿using Microsoft.AspNetCore.Http;
using ServicesAPI.DataPresentation.ErrorHandling;

namespace ServicesAPI.Services.Abstractions
{
    public interface IServerService
    {
        string GetHangfireHref(HttpContext httpContext);
        string GetServerURL(HttpContext httpContext);
        void LogJavascriptError(JavascriptError javascriptError);
    }
}
