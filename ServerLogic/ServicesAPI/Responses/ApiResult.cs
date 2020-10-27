using Newtonsoft.Json;

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
        public string Message { get; set; }

        public ApiResult() { }

        public ApiResult(ApiResultStatus apiResultStatus, string message = null)
        {
            SetResult(apiResultStatus, message);
        }

        public void SetResult(ApiResultStatus apiResultStatus, string message = null)
        {
            ApiResultStatus = apiResultStatus;
            Message = message;
        }
    }
}
