using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Application.Commands
{
    public partial class DeleteCountryCommand
    {
        /// <summary>
        /// Handler for processing the <see cref="DeleteCountryCommand"/>. It validates the request and deletes the specified country if it exists.
        /// </summary>
        protected class DeleteCountryCommandHandler
            : CommandHandlerBase<DeleteCountryCommand>, IRequestHandler<DeleteCountryCommand, OneOf<Success, OneOf.Types.NotFound, ProblemDetails>>
        {
            private readonly ICountryRepository _countryRepository;

            /// <summary>
            /// Initializes a new instance of the <see cref="DeleteCountryCommandHandler"/> class.
            /// </summary>
            /// <param name="logger">The logger instance used for logging.</param>
            /// <param name="repository">The repository for interacting with the country domain model.</param>
            /// <param name="validator">The validator used to validate the incoming command.</param>
            public DeleteCountryCommandHandler(ILogger<DeleteCountryCommandHandler> logger, ICountryRepository repository, IValidator<DeleteCountryCommand> validator)
                : base(ArgumentGuard.NotNull(validator, nameof(validator)), ArgumentGuard.NotNull(logger, nameof(logger)))
            {
                _countryRepository = ArgumentGuard.NotNull(repository, nameof(repository));
            }

            /// <summary>
            /// Handles the execution of the <see cref="DeleteCountryCommand"/>.
            /// Validates the command and, if successful, deletes the country from the repository.
            /// </summary>
            /// <param name="request">The <see cref="DeleteCountryCommand"/> request object containing the ID of the country to be deleted.</param>
            /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
            /// <returns>A <see cref="OneOf{T0, T1, T2}"/> result containing either a <see cref="Success"/>, <see cref="NotFound"/>, or a <see cref="ProblemDetails"/> if the operation fails.</returns>
            public async Task<OneOf<Success, OneOf.Types.NotFound, ProblemDetails>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
            {
                ArgumentGuard.NotNull(request, nameof(request));
                ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

                // Validate the command
                var validationResult = await Validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
                if (!validationResult.IsValid)
                {
                    return validationResult.ToProblemDetails();
                }

                // Get the country entity by ID
                var entity = await _countryRepository.GetById(request.CountryId, cancellationToken).ConfigureAwait(true);
                if (entity is null)
                {
                    return new NotFound();
                }

                try
                {
                    // Perform the delete operation
                    _countryRepository.Delete(entity);
                    entity.Delete();
                    await _countryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(true);
                    return new Success();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error deleting country");

                    return new ProblemDetails
                    {
                        Title = "Error deleting country",
                        Detail = ex.Message,
                        Status = StatusCodes.Status500InternalServerError
                    };
                }
            }
        }
    }
}
