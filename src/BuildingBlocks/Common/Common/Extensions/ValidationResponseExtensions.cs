using Common.Validation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for converting FluentValidation results into ProblemDetails for use in API responses.
    /// </summary>
    public static class ValidationResponseExtensions
    {
        /// <summary>
        /// Converts a <see cref="ValidationResult"/> into a <see cref="ProblemDetails"/> object that contains
        /// information about the validation errors.
        /// </summary>
        /// <param name="validationResult">The <see cref="ValidationResult"/> that contains the validation errors.</param>
        /// <param name="instance">An optional string that indicates the specific request instance (e.g., the request path).</param>
        /// <returns>A <see cref="ProblemDetails"/> object that contains the validation errors and relevant metadata.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validationResult"/> is <c>null</c>.</exception>
        public static ProblemDetails ToProblemDetails(this ValidationResult validationResult, string? instance = null)
        {
            var details = new ValidationProblemDetails()
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "See the errors property for details.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Instance = instance ?? string.Empty
            };

            ArgumentGuard.NotNull(validationResult, nameof(validationResult));
            var errors = validationResult.ToDictionary();
            foreach (var (key, value) in errors)
            {
                details.Errors.Add(key, value);
            }

            return details;
        }
    }
}
