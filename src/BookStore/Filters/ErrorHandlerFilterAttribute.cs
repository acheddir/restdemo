using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using ValidationException = BookStore.Exceptions.ValidationException;

namespace BookStore.Filters
{
    public class ErrorHandlerFilterAttribute : ExceptionFilterAttribute
    {
        protected readonly IDictionary<Type, Action<ExceptionContext>> ExceptionHandlers;

        public ErrorHandlerFilterAttribute()
        {
            ExceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(ValidationException), HandleValidationException },
            };
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = (ValidationException) context.Exception;

            ProblemDetails details;

            if (exception.Errors.Count > 0)
            {
                details = new ValidationProblemDetails(exception.Errors)
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
                };
            }
            else
            {
                details = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = exception.Message,
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
                };
            }

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var details = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "The specified resource was not found.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();

            if (ExceptionHandlers.ContainsKey(exceptionType))
            {
                ExceptionHandlers[exceptionType].Invoke(context);
            }
        }
    }
}
