using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendorConfiguration.Application.Dto;
using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Application.Commands
{
    public partial class CreateCountryCommand
    {
        /// <summary>
        /// Handler for processing the <see cref="CreateCountryCommand"/>. It validates the request and adds the country to the database if it is unique.
        /// </summary>
        protected class CreateCountryCommandHandler
            : CommandHandlerBase<CreateCountryCommand>, IRequestHandler<CreateCountryCommand, OneOf<CountryDto, ProblemDetails>>
        {
            private readonly IMapper _mapper;
            private readonly ICountryRepository _repository;

            /// <summary>
            /// Initializes a new instance of the <see cref="CreateCountryCommandHandler"/> class.
            /// </summary>
            /// <param name="mapper">The AutoMapper instance used to map between domain entities and DTOs.</param>
            /// <param name="logger">The logger instance used to log errors and information.</param>
            /// <param name="repository">The repository for interacting with the country domain model.</param>
            /// <param name="validator">The validator used to validate the incoming command.</param>
            public CreateCountryCommandHandler(IMapper mapper, ILogger<CreateCountryCommandHandler> logger, ICountryRepository repository, IValidator<CreateCountryCommand> validator)
                : base(ArgumentGuard.NotNull(validator, nameof(validator)), ArgumentGuard.NotNull(logger, nameof(logger)))
            {
                _mapper = ArgumentGuard.NotNull(mapper, nameof(mapper));
                _repository = ArgumentGuard.NotNull(repository, nameof(repository));
            }

            /// <summary>
            /// Handles the execution of the <see cref="CreateCountryCommand"/>.
            /// Validates the command and, if successful, creates a new country in the repository.
            /// </summary>
            /// <param name="request">The <see cref="CreateCountryCommand"/> request object containing the details of the country to be created.</param>
            /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
            /// <returns>A <see cref="OneOf{T0, T1}"/> result containing either a <see cref="CountryDto"/> or a <see cref="ProblemDetails"/> if the operation fails.</returns>
            public async Task<OneOf<CountryDto, ProblemDetails>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
            {
                ArgumentGuard.NotNull(request, nameof(request));
                ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

                var validationResult = await Validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
                switch (validationResult.IsValid)
                {
                    case false:
                        return validationResult.ToProblemDetails();
                    default:
                        {
                            var data = request.RequestData;
                            try
                            {
                                if (await _repository.IsCountryUnique(data.Iso3166CountryCode2, data.Iso3166CountryCode3, cancellationToken).ConfigureAwait(true))
                                {
                                    var entity = _mapper.Map<Country>(request);
                                    await _repository.Add(entity).ConfigureAwait(true);
                                    await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(true);
                                    return _mapper.Map<CountryDto>(entity);
                                }

                                return new ProblemDetails
                                {
                                    Detail = $"The country codes '{data.Iso3166CountryCode2}' and/or '{data.Iso3166CountryCode3}' are already in the database",
                                };
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, "Error creating country");

                                return new ProblemDetails
                                {
                                    Title = "Error creating country",
                                    Detail = ex.Message,
                                    Status = StatusCodes.Status500InternalServerError
                                };
                            }
                        }
                }
            }
        }
    }
}