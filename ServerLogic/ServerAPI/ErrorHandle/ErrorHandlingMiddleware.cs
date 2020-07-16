using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServerAPI.ApiResponses;
using ServicesAPI.ErrorHandle.ApiExceptions;
using System;
using System.Threading.Tasks;

namespace ServerAPI.ErrorHandle
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var stackTrace = string.Empty;
            ApiError apiError = null;

            var exceptionType = exception.GetType();

            if (exceptionType == typeof(BadRequestException))
            {
                apiError = new BadRequestApiError(exception.Message);
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                apiError = new NotFoundApiError(exception.Message);
            }
            else
            {
                apiError = new InternalServerApiError(exception.Message);
            }

            logger.LogError("An error occured", $"Exception: ${exception.Message}", $"Stack trace: ${exception.StackTrace}");

            var result = apiError.ToString();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = apiError.StatusCode;

            return context.Response.WriteAsync(result);
        }
    }
}
