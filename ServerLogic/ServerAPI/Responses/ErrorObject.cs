using System.Collections.Generic;

namespace ServerAPI.Responses
{
    public class ErrorObject
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public ErrorObject()
        {

        }
        public ErrorObject(string message, IEnumerable<string> errors)
        {
            Message = message;
            Errors = errors;
        }
    }
}
