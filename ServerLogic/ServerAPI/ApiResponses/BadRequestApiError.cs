using System.Net;

namespace ServerAPI.ApiResponses
{
    public class BadRequestApiError : ApiError
    {
        public BadRequestApiError() : base(400, HttpStatusCode.BadRequest.ToString())
        {

        }
        public BadRequestApiError(string message) : base(400, HttpStatusCode.BadRequest.ToString(), message)
        {

        }
    }
}
