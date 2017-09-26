using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Common;
using Common.ValueObjects;
using System.Linq;

namespace Cookbook.Api.Infrastructure
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly bool _shieldInternalMessages;

        public ApiExceptionFilterAttribute(bool shieldInternalMessages)
        {
            _shieldInternalMessages = shieldInternalMessages;
        }

        public override void OnException(ExceptionContext context)
        {
            var acceptHeader = context.HttpContext.Request.Headers["Accept"].FirstOrDefault()?.Split(',') ?? new string[0];
            if (!(acceptHeader.Any(x => x.StartsWith("application/json") || x.StartsWith("*/*"))))
            {
                base.OnException(context);
                return;
            }

            ErrorMessage result = null;
            if (context.Exception is ApiException apiException)
            {
                result = MapToMessage(apiException);
            }
            else
            {
                result = new ErrorMessage
                {
                    Error = 500,
                    SubError = 0,
                    MessageKey = "InternalError",
                    Message = _shieldInternalMessages ? "Internal Error" : context.Exception.ToString()
                };
            }

            context.Result = new JsonResult(result)
            {
                StatusCode = result.Error
            };
            context.ExceptionHandled = true;
        }

        private ErrorMessage MapToMessage(ApiException ex)
        {
            return new ErrorMessage
            {
                Error = ex.Error,
                SubError = ex.SubError,
                MessageKey = ex.MessageKey,
                Message = ex.Message,
                Details = ex.Details.Any() ? ex.Details.Select(x => MapToMessage(x)).ToArray() : null
            };
        }
    }
}
