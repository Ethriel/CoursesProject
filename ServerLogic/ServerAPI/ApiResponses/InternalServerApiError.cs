using System.Net;

namespace ServerAPI.ApiResponses
{
    public class InternalServerApiError : ApiError
    {
        public string StackTrace { get; private set; }
        public InternalServerApiError()
            : base(500, HttpStatusCode.InternalServerError.ToString())
        {
        }

        public InternalServerApiError(string message)
            : base(500, HttpStatusCode.InternalServerError.ToString(), message)
        {
        }
    }
}
