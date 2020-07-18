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
                    logger.LogInformation(result.LoggerMessage);
                    return controller.Ok(result.Data);
                case ApiResultStatus.NotFound:
                    logger.LogWarning(result.LoggerMessage);
                    return controller.NotFound(result);
                case ApiResultStatus.BadRequest:
                default:
                    logger.LogWarning(result.LoggerMessage);
                    return controller.BadRequest(result);
            }
        }
    }
}
