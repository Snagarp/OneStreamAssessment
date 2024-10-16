using Common.Extensions;
using Common.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Behaviors
{
    /// <summary>
    /// Pipeline behavior for validating requests in the MediatR pipeline.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="validators">The collection of validators for the request.</param>
        /// <param name="logger">The logger to log validation information.</param>
        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
        {
            _validators = ArgumentGuard.NotNull(validators);
            _logger = ArgumentGuard.NotNull(logger);
        }

        /// <summary>
        /// Handles the validation of the request before the next delegate in the pipeline is invoked.
        /// </summary>
        /// <param name="request">The request being processed.</param>
        /// <param name="next">The delegate representing the next action in the pipeline.</param>
        /// <param name="cancellationToken">A token to cancel the request handling.</param>
        /// <returns>The response from the next action in the pipeline.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ArgumentGuard.NotNull(request, nameof(request));
            ArgumentGuard.NotNull(next, nameof(next));
            ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

            var typeName = request.GetGenericTypeName();
            _logger.LogInformation("----- Validating command {CommandType}", typeName);

            // Perform validation and collect any failures.
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Count != 0)
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);
            }

            // Proceed to the next action in the pipeline.
            return await next().ConfigureAwait(true);
        }
    }
}
