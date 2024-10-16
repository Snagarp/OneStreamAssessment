using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace VendorConfiguration.Api.Infrastructure.Handlers
{
    /// <summary>
    /// A delegating handler for logging HTTP client requests and responses.
    /// </summary>
    public class HttpClientLoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientLoggingHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger used to log requests and responses.</param>
        public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger)
        {
            _logger = ArgumentGuard.NotNull(logger, nameof(logger));
        }

        /// <summary>
        /// Sends an HTTP request, logging the request and response details.
        /// </summary>
        /// <param name="request">The HTTP request to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Request: {request}", request.ToString());

            if (request.Content is not null)
            {
                var content = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Request Content: {content}", content);
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug("Response: {response}", response.ToString());

            if (response.Content is not null)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Response Content: {content}", content);
            }

            return response;
        }
    }
}
