using Common.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Common.Http
{
    /// <summary>
    /// A delegating handler that attaches authorization headers from the current HTTP context to outgoing HTTP requests.
    /// </summary>
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientAuthorizationDelegatingHandler"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor to retrieve the current request context.</param>
        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = ArgumentGuard.NotNull(httpContextAccessor);
        }

        /// <summary>
        /// Adds authorization headers to the outgoing HTTP request if they exist in the current HTTP context.
        /// </summary>
        /// <param name="request">The outgoing HTTP request message.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentGuard.NotNull(request, nameof(request));
            ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

            // Retrieve headers from the current HTTP context
            var headers = _httpContextAccessor!.HttpContext!.Request?.Headers;
            if (headers is not null)
            {
                // Add Authorization header if it exists
                var authorizationHeader = headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(authorizationHeader))
                {
                    request.Headers.Add("Authorization", new List<string>() { authorizationHeader! });
                }

                // Add JwtToken header for assessment validation
                var jwtToken = GetJWTTokenAsyncforAssessment(headers);
                if (jwtToken != null)
                {
                    request.Headers.Add("JwtToken", jwtToken);
                }
            }

            // Retrieve the access token and add it to the Authorization header if it exists
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Proceed with sending the request
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the access token from the current HTTP context.
        /// </summary>
        /// <returns>The access token, if available; otherwise, null.</returns>
        private async Task<string?> GetTokenAsync()
        {
            const string ACCESS_TOKEN = "access_token";
            return await _httpContextAccessor!.HttpContext!.GetTokenAsync(ACCESS_TOKEN).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the JWT token from the HTTP headers for assessment validation.
        /// </summary>
        /// <param name="headers">The HTTP request headers.</param>
        /// <returns>The JWT token, if available; otherwise, null.</returns>
        private static string? GetJWTTokenAsyncforAssessment(IHeaderDictionary headers)
        {
            const string JWT_TOKEN = "JwtToken";
            return headers[JWT_TOKEN];
        }
    }
}
