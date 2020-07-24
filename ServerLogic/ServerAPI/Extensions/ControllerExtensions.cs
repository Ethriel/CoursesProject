using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAPI.Responses;
using ServicesAPI.Responses.AccountResponseData;

namespace ServerAPI.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult GetActionResult(this Controller controller, ApiResult result, ILogger logger)
        {
            switch (result.ApiResultStatus)
            {
                case ApiResultStatus.Ok:
                    if (result.Data is AccountData)
                    {
                        logger.LogInformation(result.LoggerMessage);
                    }
                    return controller.Ok(result);
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
