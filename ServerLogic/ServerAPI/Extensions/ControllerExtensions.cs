using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAPI.Responses;

namespace ServerAPI.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult GetActionResult(this Controller controller, ApiResult result, ILogger logger)
        {
            switch (result.ApiResultStatus)
            {
                case ApiResultStatus.Ok:
                    return controller.Ok(result);
                case ApiResultStatus.NotFound:
                    logger.LogWarning((result as ApiErrorResult).LoggerMessage);
                    return controller.NotFound(result);
                case ApiResultStatus.BadRequest:
                default:
                    logger.LogWarning((result as ApiErrorResult).LoggerMessage);
                    return controller.BadRequest(result);
            }
        }
    }
}
