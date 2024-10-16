using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VendorConfiguration.Application.Extensions
{
    /// <summary>
    /// Provides helper methods for creating standardized <see cref="ProblemDetails"/> responses.
    /// </summary>
    public static class ProblemDetailHelpers
    {
        /// <summary>
        /// Creates a <see cref="ProblemDetails"/> object for a 404 Not Found error.
        /// </summary>
        /// <param name="detail">The detailed error message describing why the resource was not found.</param>
        /// <returns>A <see cref="ProblemDetails"/> object with a 404 status code.</returns>
        public static ProblemDetails NotFound(string detail) => new()
        {
            Title = "Resource Not found",
            Detail = detail,
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }
}
