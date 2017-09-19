using System;

namespace Common
{
    public class ApiException : ApplicationException
    {
        public ApiException(int error, int subError, string message, string messageKey, params object[] args)
            : base(message)
        {
            Error = error;
            SubError = subError;
            MessageKey = messageKey;
            Arguments = args;
        }

        public int Error { get; private set; }
        public int SubError { get; private set; }
        public string MessageKey { get; private set; }
        public object[] Arguments { get; private set; }

        public ApiException[] Details { get; protected set; } = new ApiException[0];
    }
}
