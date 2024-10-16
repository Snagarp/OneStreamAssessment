using System.Net;
using Common.ActionResults;
using Common.Exceptions;
using Common.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Common.Filters
{
    /// <summary>
    /// A global exception filter that handles exceptions across the application and returns appropriate HTTP responses.
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGlobalExceptionFilter"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log errors and exceptions.</param>
        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.logger = ArgumentGuard.NotNull(logger);
        }

        /// <summary>
        /// Handles exceptions thrown in the application and formats appropriate HTTP responses.
        /// </summary>
        /// <param name="context">The exception context containing information about the thrown exception.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
        public void OnException(ExceptionContext context)
        {
            ArgumentGuard.NotNull(context, nameof(context));

            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            if (context.Exception.GetType().GetInterface(nameof(IDomainException)) == typeof(IDomainException))
            {
                HandleDomainException(context);
            }
            else
            {
                HandleInternalServerError(context);
            }

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Handles domain-specific exceptions and returns a 400 Bad Request response.
        /// </summary>
        /// <param name="context">The exception context containing information about the thrown exception.</param>
        private void HandleDomainException(ExceptionContext context)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };

            switch (context.Exception.InnerException)
            {
                case ValidationException validationException:
                    problemDetails.Errors.Add("DomainValidations", validationException.Errors.Select(err => err.ErrorMessage).ToArray());
                    break;
                default:
                    {
                        var errors = new List<string> { context.Exception.Message };

                        if (context.Exception.GetType().GetInterface(nameof(IHasAdditionalData)) == typeof(IHasAdditionalData))
                        {
                            errors.AddRange(context.Exception is IHasAdditionalData additionalDataException ? additionalDataException.GetAdditionalData() : Array.Empty<string>());
                        }

                        problemDetails.Errors.Add("DomainValidations", errors.ToArray());
                        break;
                    }
            }

            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Handles non-domain exceptions and returns a 500 Internal Server Error response.
        /// </summary>
        /// <param name="context">The exception context containing information about the thrown exception.</param>
        private static void HandleInternalServerError(ExceptionContext context)
        {
            var jsonResponse = new JsonErrorResponse
            {
                Messages = "An error occurred. Please try again.",
                DeveloperMessage = $"{context.Exception.Message} {context.Exception.InnerException?.Message ?? string.Empty}"
            };

            context.Result = new InternalServerErrorObjectResult(jsonResponse);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Represents a JSON error response returned to the client.
        /// </summary>
        private class JsonErrorResponse
        {
            /// <summary>
            /// Gets or sets the user-facing error messages.
            /// </summary>
            public string? Messages { get; set; }

            /// <summary>
            /// Gets or sets the developer-facing error message for debugging purposes.
            /// </summary>
            public string? DeveloperMessage { get; set; }
        }
    }
}
