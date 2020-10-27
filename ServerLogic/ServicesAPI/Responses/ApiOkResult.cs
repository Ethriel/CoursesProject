namespace ServicesAPI.Responses
{
    public class ApiOkResult : ApiResult
    {
        public object Data { get; set; }

        public ApiOkResult()
        {

        }

        public ApiOkResult(ApiResultStatus apiResultStatus, object data = null)
        {
            SetOkResult(apiResultStatus, data);
        }
        public void SetOkResult(ApiResultStatus apiResultStatus, object data = null)
        {
            ApiResultStatus = apiResultStatus;
            Data = data;
        }
    }
}
