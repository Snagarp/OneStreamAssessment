using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.ActionResults
{
    /// <summary>
    /// Represents an ObjectResult that produces a 500 Internal Server Error response.
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorObjectResult"/> class.
        /// </summary>
        /// <param name="error">The error object to include in the response.</param>
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            // Sets the status code to 500 (Internal Server Error)
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}