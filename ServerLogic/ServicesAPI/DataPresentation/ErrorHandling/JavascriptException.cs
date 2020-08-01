using System;

namespace ServicesAPI.DataPresentation.ErrorHandling
{
    public class JavascriptException : Exception
    {
        private readonly string stackTrace;

        public JavascriptException(string message, string stackTrace) : base(message)
        {
            this.stackTrace = stackTrace;
        }

        public override string StackTrace
        {
            get
            {
                return stackTrace;
            }
        }
    }
}
