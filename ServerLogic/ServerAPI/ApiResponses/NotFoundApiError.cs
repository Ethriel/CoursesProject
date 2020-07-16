using System.Net;

namespace ServerAPI.ApiResponses
{
    public class NotFoundApiError : ApiError
    {
        public NotFoundApiError()
            : base(404, HttpStatusCode.NotFound.ToString())
        {
        }
        public NotFoundApiError(string message)
            : base(404, HttpStatusCode.NotFound.ToString(), message)
        {
        }
    }
}
