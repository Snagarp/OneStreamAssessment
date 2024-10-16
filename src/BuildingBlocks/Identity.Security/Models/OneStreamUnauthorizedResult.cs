using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Security.Models
{
    /// <summary>
    /// Represents a custom JSON result that returns a 401 Unauthorized status code along with a message.
    /// </summary>
    public class OneStreamUnauthorizedResult : JsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OneStreamUnauthorizedResult"/> class with the specified message.
        /// </summary>
        /// <param name="message">The message to be included in the response body.</param>
        public OneStreamUnauthorizedResult(string message)
            : base(new { message })
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}