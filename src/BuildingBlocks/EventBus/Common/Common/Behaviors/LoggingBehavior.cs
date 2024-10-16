//2023 (c) TD Synnex - All Rights Reserved.

using Common.Extensions;
using Common.Validation;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Common.Behaviors;
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

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

