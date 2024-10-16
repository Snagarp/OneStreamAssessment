using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VendorConfiguration.Application.Commands
{
    /// <summary>
    /// Base class for handling commands, providing validation and logging functionality.
    /// </summary>
    /// <typeparam name="TRequest">The type of the command that extends <see cref="CommandBase"/>.</typeparam>
    public abstract class CommandHandlerBase<TRequest> where TRequest : CommandBase
    {
        /// <summary>
        /// Gets the validator responsible for validating the command.
        /// </summary>
        protected IValidator<TRequest> Validator { get; private set; }

        /// <summary>
        /// Gets the logger used for logging command execution details.
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerBase{TRequest}"/> class.
        /// </summary>
        /// <param name="validator">The validator used to validate the command.</param>
        /// <param name="logger">The logger used for logging command execution details.</param>
        protected CommandHandlerBase(IValidator<TRequest> validator, ILogger logger)
        {
            Validator = ArgumentGuard.NotNull(validator, nameof(validator));
            Logger = ArgumentGuard.NotNull(logger, nameof(logger));
        }

        /// <summary>
        /// Validates the command asynchronously using the provided validator.
        /// </summary>
        /// <param name="command">The command to validate.</param>
        /// <param name="stoppingToken">The cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="OneOf{T0,T1}"/> indicating success or a <see cref="ProblemDetails"/> object with validation issues.</returns>
        protected async Task<OneOf<Success, ProblemDetails>> Validate(TRequest command, CancellationToken stoppingToken)
        {
            var validationResult = await Validator.ValidateAsync(command, stoppingToken).ConfigureAwait(false);
            return validationResult.IsValid switch
            {
                true => (OneOf<Success, ProblemDetails>)new Success(),
                _ => (OneOf<Success, ProblemDetails>)validationResult.ToProblemDetails(),
            };
        }

        /// <summary>
        /// Creates a <see cref="ProblemDetails"/> object representing a 404 Not Found response.
        /// </summary>
        /// <param name="message">The message detailing why the entity was not found.</param>
        /// <returns>A <see cref="ProblemDetails"/> object with a 404 Not Found status code.</returns>
        protected ProblemDetails NotFound(string message) => new()
        {
            Detail = ArgumentGuard.NotNullOrEmpty(message, nameof(message)),
            Status = StatusCodes.Status404NotFound
        };
    }
}
