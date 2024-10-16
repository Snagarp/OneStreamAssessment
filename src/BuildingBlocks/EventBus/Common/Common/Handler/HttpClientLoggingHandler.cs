//2023 (c) TD Synnex - All Rights Reserved.

using Common.Validation;

using Microsoft.Extensions.Logging;

namespace Common.Handler
{
    public class HttpClientLoggingHandler
     : DelegatingHandler
    {
        private readonly ILogger _logger;

        public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger)
        {
            _logger = ArgumentGuard.NotNull(logger, nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentGuard.NotNull(request, nameof(request));
            _logger.LogDebug("Request: {Request}", request.ToString());

            if (request.Content is not null)
            {
                var content = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Request Content: {Content}", content);
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug("response: {Response}", response.ToString());

            if (response.Content is not null)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Response Content: {Content}", content);

            }

            return response;
        }
    }
}
