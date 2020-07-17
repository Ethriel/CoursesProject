using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAPI.Responses;

namespace ServerAPI.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult GetActionResult(this Controller controller, ApiResult result, ILogger logger)
        {
            logger.LogInformation(result.LoggerMessage);

            switch (result.ApiResultStatus)
            {
                case ApiResultStatus.Ok:
                    return controller.Ok(result.Data);
                case ApiResultStatus.NotFound:
                    return controller.NotFound(result);
                case ApiResultStatus.BadRequest:
                default:
                    return controller.BadRequest(result);
            }
        }
    }
}
