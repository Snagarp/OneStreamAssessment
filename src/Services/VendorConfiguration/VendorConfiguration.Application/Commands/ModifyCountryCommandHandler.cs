using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorConfiguration.Application.Dto;
using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Application.Commands
{
    public partial class ModifyCountryCommand
    {
        /// <summary>
        /// Handler responsible for processing the modification of an existing country.
        /// </summary>
        protected class ModifyCountryCommandHandler
            : CommandHandlerBase<ModifyCountryCommand>, IRequestHandler<ModifyCountryCommand, OneOf<CountryDto, NotFound, ProblemDetails>>
        {
            private readonly ICountryRepository _repository;
            private readonly IMapper _mapper;

            /// <summary>
            /// Initializes a new instance of the <see cref="ModifyCountryCommandHandler"/> class.
            /// </summary>
            /// <param name="logger">Logger to capture the log details.</param>
            /// <param name="repository">Country repository for managing country data operations.</param>
            /// <param name="mapper">Mapper to map between domain entities and DTOs.</param>
            /// <param name="validator">Validator to validate the request data.</param>
            public ModifyCountryCommandHandler(ILogger<ModifyCountryCommandHandler> logger, ICountryRepository repository, IMapper mapper, IValidator<ModifyCountryCommand> validator)
                : base(ArgumentGuard.NotNull(validator, nameof(validator)), ArgumentGuard.NotNull(logger, nameof(logger)))
            {
                _repository = ArgumentGuard.NotNull(repository, nameof(repository));
                _mapper = ArgumentGuard.NotNull(mapper, nameof(mapper));
            }

            /// <summary>
            /// Handles the country modification command.
            /// </summary>
            /// <param name="request">The request containing the country modification data.</param>
            /// <param name="cancellationToken">Cancellation token to observe cancellation requests.</param>
            /// <returns>A <see cref="OneOf{CountryDto, NotFound, ProblemDetails}"/> containing the result of the modification operation.</returns>
            public async Task<OneOf<CountryDto, NotFound, ProblemDetails>> Handle(ModifyCountryCommand request, CancellationToken cancellationToken)
            {
                ArgumentGuard.NotNull(request, nameof(request));
                ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

                // Validate the request
                var validationResult = await Validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
                if (!validationResult.IsValid)
                {
                    return validationResult.ToProblemDetails();
                }

                // Fetch the country entity by ID
                var entity = await _repository.GetById(request.CountryId, cancellationToken).ConfigureAwait(true);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.
                if (entity == null)
                {
                    return new NotFound();
                }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                // Check for uniqueness of the country code during modification
                var data = request.RequestData;
                if (!string.IsNullOrEmpty(data.Iso3166CountryCode2) || !string.IsNullOrEmpty(data.Iso3166CountryCode3))
                {
                    if (!await _repository.IsCountryUniqueOnModify(entity, data.Iso3166CountryCode2, data.Iso3166CountryCode3, cancellationToken).ConfigureAwait(false))
                    {
                        return new ProblemDetails
                        {
                            Title = "Country already exists",
                            Detail = $"The country codes '{data.Iso3166CountryCode2}' and/or '{data.Iso3166CountryCode3}' are already in the database"
                        };
                    }
                }

                try
                {
                    // Map the request data to the country entity and update it
                    _mapper.Map<ModifyCountryCommand, Country>(request, entity);
                    _repository.Update(entity);
                    entity.Modify();
                    await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(true);

                    // Return the modified country as a DTO
                    return _mapper.Map<CountryDto>(entity);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error modifying country");

                    return new ProblemDetails
                    {
                        Title = "Error modifying country",
                        Detail = ex.Message,
                        Status = StatusCodes.Status500InternalServerError
                    };
                }
            }
        }
    }
}