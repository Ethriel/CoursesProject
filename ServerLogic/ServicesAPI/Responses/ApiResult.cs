using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServicesAPI.Responses
{
    public enum ApiResultStatus
    {
        Ok,
        NotFound,
        BadRequest
    }
    public class ApiResult
    {
        [JsonIgnore]
        public ApiResultStatus ApiResultStatus { get; set; }
        public object Data { get; set; }
        [JsonIgnore]
        public string LoggerMessage { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public ApiResult() { }
        public ApiResult(ApiResultStatus apiResultStatus, string loggerMessage = null, object data = null, string message = null, IEnumerable<string> errors = null)
        {
            ApiResultStatus = apiResultStatus;
            Data = data;
            LoggerMessage = loggerMessage;
            Message = message;
            Errors = errors;
        }
        public void SetApiResult(ApiResultStatus apiResultStatus, string loggerMessage = null, object data = null, string message = null, IEnumerable<string> errors = null)
        {
            ApiResultStatus = apiResultStatus;
            Data = data;
            LoggerMessage = loggerMessage;
            Message = message;
            Errors = errors;
        }
    }
}
