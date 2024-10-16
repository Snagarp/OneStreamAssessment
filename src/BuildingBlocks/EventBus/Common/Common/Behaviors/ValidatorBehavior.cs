//2023 (c) TD Synnex - All Rights Reserved.

using Common.Extensions;
using Common.Validation;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Common.Behaviors;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
    {
        _validators = ArgumentGuard.NotNull(validators);
        _logger = ArgumentGuard.NotNull(logger);
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentGuard.NotNull(request, nameof(request));
        ArgumentGuard.NotNull(next, nameof(next));
        ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

        if (next is null) throw new ArgumentNullException(nameof(next));
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation("----- Validating command {CommandType}", typeName);

        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);
        }

        return await next().ConfigureAwait(true);
    }
}
