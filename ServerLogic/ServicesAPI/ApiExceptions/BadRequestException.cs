using System;

namespace ServicesAPI.ErrorHandle.ApiExceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
