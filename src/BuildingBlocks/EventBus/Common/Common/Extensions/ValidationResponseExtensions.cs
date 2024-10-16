//2023 (c) TD Synnex - All Rights Reserved.
#pragma warning disable CA1062

using Common.Validation;

using FluentValidation.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Common.Extensions
{
    public static class ValidationResponseExtensions
    {
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
