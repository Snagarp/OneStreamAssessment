using Common.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Nodes;

namespace Common.ActionResults
{
    /// <summary>
    /// A utility class to map dynamic results to appropriate <see cref="ObjectResult"/> based on the HTTP status code.
    /// </summary>
    public static class ObjectResultMapper
    {
        /// <summary>
        /// Maps a dynamic result to an <see cref="ObjectResult"/> with the appropriate status code.
        /// </summary>
        /// <param name="result">The dynamic result to be mapped.</param>
        /// <returns>An <see cref="ObjectResult"/> representing the result with the appropriate HTTP status code.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the result is null.</exception>
        public static ObjectResult GetResult(dynamic result)
        {
            ArgumentGuard.NotNull(result, nameof(result));

            int status = GetResponseStatus(result);

            return status switch
            {
                400 => new BadRequestObjectResult(result),
                500 => new InternalServerErrorObjectResult(result),
                _ => new OkObjectResult(result),
            };
        }

        /// <summary>
        /// Extracts the HTTP status code from a <see cref="JsonNode"/> object.
        /// </summary>
        /// <param name="result">The JSON node containing the result data.</param>
        /// <returns>The HTTP status code derived from the result, or <see cref="HttpStatusCode.OK"/> by default.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the result is null.</exception>
        private static int GetResponseStatus(JsonNode result)
        {
            ArgumentGuard.NotNull(result, nameof(result));

            int status = (int)HttpStatusCode.OK;

            if (result.GetType() == typeof(JsonObject))
            {
                status = result["status"]?.GetValue<short>() ?? status;
            }

            return status;
        }
    }
}
