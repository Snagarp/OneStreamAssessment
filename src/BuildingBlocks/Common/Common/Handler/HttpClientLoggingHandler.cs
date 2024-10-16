using Common.Validation;
using Microsoft.Extensions.Logging;

namespace Common.Handler
{
    /// <summary>
    /// A custom HTTP client delegating handler that logs the request and response details.
    /// </summary>
    public class HttpClientLoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientLoggingHandler"/> class.
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger"/> to log request and response details.</param>
        /// <exception cref="ArgumentNullException">Thrown when the logger is null.</exception>
        public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger)
        {
            _logger = ArgumentGuard.NotNull(logger, nameof(logger));
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler and logs the request and response details.
        /// </summary>
        /// <param name="request">The HTTP request message to be sent.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, containing the HTTP response message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is null.</exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentGuard.NotNull(request, nameof(request));

            // Log the HTTP request details
            _logger.LogDebug("Request: {Request}", request.ToString());

            // If there is request content, log it
            if (request.Content is not null)
            {
                var content = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Request Content: {Content}", content);
            }

            // Call the base handler to send the request and get the response
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            // Log the HTTP response details
            _logger.LogDebug("Response: {Response}", response.ToString());

            // If there is response content, log it
            if (response.Content is not null)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Response Content: {Content}", content);
            }

            return response;
        }
    }
}
