using Common.Extensions;
using Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Behaviors
{
    /// <summary>
    /// Logging behavior for MediatR pipeline that logs requests and responses.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger to log request and response information.</param>
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        /// <summary>
        /// Handles logging of the request and response in the MediatR pipeline.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="next">The delegate representing the next action in the pipeline.</param>
        /// <param name="cancellationToken">A token to cancel the request handling.</param>
        /// <returns>The response from the next action in the pipeline.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ArgumentGuard.NotNull(request, nameof(request));
            ArgumentGuard.NotNull(next, nameof(next));

            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), request);
            var response = await next().ConfigureAwait(false);
            _logger.LogInformation("----- Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), response);

            return response;
        }
    }
}
