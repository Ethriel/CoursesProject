using System;

namespace ServicesAPI.ErrorHandle.ApiExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
