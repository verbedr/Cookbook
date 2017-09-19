using Common;

namespace Cookbook.Domain.Exceptions
{
    public class CookbookException : ApiException
    {
        public CookbookException(int error, int subError, string messageKey, params object[] args)
            : base(error, subError,
                  string.Format(Messages.ResourceManager.GetString($"Error_{error}_{subError}_{messageKey}")
                                    ?? $"{error}.{subError} - {messageKey}", args ?? new object[0]),
                  messageKey, args)
        {
        }

    }
}
