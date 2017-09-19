namespace Cookbook.Domain.Exceptions
{
    public class ValidationException : CookbookException
    {
        public ValidationException(ValidationErrors subError, string messageKey = null, params object[] args)
            : base(400, (int)subError, messageKey ?? "Default", args)
        {
        }

        public ValidationException(ValidationException[] details)
            : base(400, (int)ValidationErrors.Grouped, "Default")
        {
            Details = details ?? new ValidationException[0];
        }

        public enum ValidationErrors
        {
            Generic = 0,
            RequiredField = 1,
            TooLong = 2,
            TooShort = 3,
            UnknownItem = 4,
            InvalidValue = 5,
            DateOutOfRange = 6,
            Readonly = 7,
            Grouped = 99,
        }
    }
}
