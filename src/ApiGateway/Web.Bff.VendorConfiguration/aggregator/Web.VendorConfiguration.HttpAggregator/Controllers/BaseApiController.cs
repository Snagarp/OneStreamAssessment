using Microsoft.AspNetCore.Mvc;

namespace Web.VendorConfiguration.HttpAggregator.Controllers
{
    /// <summary>
    /// A base API controller that provides common functionalities for handling HTTP responses and converting them into <see cref="IActionResult"/>.
    /// </summary>
    [Route("/api/v1/")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Converts an <see cref="HttpResponseMessage"/> to an <see cref="IActionResult"/>.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> received from the HTTP client.</param>
        /// <returns>An <see cref="IActionResult"/> corresponding to the HTTP status code in the response.</returns>
        protected async Task<IActionResult> ConvertToActionResult(HttpResponseMessage response)
        {
            ArgumentGuard.NotNull(response, nameof(response));

            var content = await GetContent(response).ConfigureAwait(false);

            return response?.StatusCode switch
            {
                System.Net.HttpStatusCode.OK => new OkObjectResult(content),
                System.Net.HttpStatusCode.Created => StatusCode((int)System.Net.HttpStatusCode.Created, content),
                System.Net.HttpStatusCode.NoContent => new NoContentResult(),
                System.Net.HttpStatusCode.NotFound => new NotFoundResult(),
                System.Net.HttpStatusCode.InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, content),
                System.Net.HttpStatusCode.BadRequest => new BadRequestObjectResult(content),
                _ => new ObjectResult(content)
            };
        }

        /// <summary>
        /// Retrieves the content from an <see cref="HttpResponseMessage"/> as a string.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> from which to extract the content.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation, with the content as a dynamic type.</returns>
        protected static async Task<dynamic> GetContent(HttpResponseMessage response)
        {
            ArgumentGuard.NotNull(response, nameof(response));
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
    }
}
