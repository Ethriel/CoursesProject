namespace ServicesAPI.Responses
{
    public class ApiOkResult : ApiResult
    {
        public object Data { get; set; }

        public ApiOkResult()
        {

        }

        public ApiOkResult(ApiResultStatus apiResultStatus, string message = null, object data = null)
        {
            SetOkResult(apiResultStatus, message, data);
        }

        public void SetOkResult(ApiResultStatus apiResultStatus, string message = null, object data = null)
        {
            ApiResultStatus = apiResultStatus;
            Data = data;
            Message = message;
        }
    }
}
