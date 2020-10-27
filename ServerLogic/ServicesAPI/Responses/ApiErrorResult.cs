using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServicesAPI.Responses
{
    public class ApiErrorResult : ApiResult
    {
        public IEnumerable<string> Errors { get; set; }
        [JsonIgnore]
        public string LoggerMessage { get; set; }

        public ApiErrorResult()
        {

        }

        public ApiErrorResult(ApiResultStatus apiResultStatus, string loggerMessage, string message = null, IEnumerable<string> errors = null)
        {
            SetErrorResult(apiResultStatus, loggerMessage, message, errors);
        }

        public void SetErrorResult(ApiResultStatus apiResultStatus, string loggerMessage, string message = null, IEnumerable<string> errors = null)
        {
            ApiResultStatus = apiResultStatus;
            LoggerMessage = loggerMessage;
            Message = message;
            Errors = errors;
        }
    }
}
