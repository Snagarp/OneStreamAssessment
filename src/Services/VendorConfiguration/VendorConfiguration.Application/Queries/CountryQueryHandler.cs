using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VendorConfiguration.Application.Commands;
using VendorConfiguration.Application.Dto;
using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Application.Queries
{
    public partial class CountryQuery
    {
        /// <summary>
        /// Handles the execution of the CountryQuery to retrieve one or more country records from the repository.
        /// </summary>
        protected class CountryQueryHandler
            : CommandHandlerBase<CountryQuery>, IRequestHandler<CountryQuery, OneOf<CountryDto, IList<CountryDto>, None, ProblemDetails>>
        {
            private readonly ICountryRepository _repository;
            private readonly IMapper _mapper;

            /// <summary>
            /// Initializes a new instance of the <see cref="CountryQueryHandler"/> class.
            /// </summary>
            /// <param name="logger">Logger to capture log messages.</param>
            /// <param name="repository">Country repository for data access.</param>
            /// <param name="mapper">Mapper instance for converting between domain and DTO objects.</param>
            /// <param name="validator">Validator for validating the incoming query.</param>
            public CountryQueryHandler(ILogger<CountryQueryHandler> logger, ICountryRepository repository, IMapper mapper, IValidator<CountryQuery> validator)
                : base(ArgumentGuard.NotNull(validator, nameof(validator)), ArgumentGuard.NotNull(logger, nameof(logger)))
            {
                _repository = ArgumentGuard.NotNull(repository);
                _mapper = ArgumentGuard.NotNull(mapper);
            }

            /// <summary>
            /// Handles the execution of the CountryQuery request.
            /// </summary>
            /// <param name="request">The country query request.</param>
            /// <param name="cancellationToken">Token used to cancel the operation.</param>
            /// <returns>A result that contains a single or multiple countries, or an error response.</returns>
            public async Task<OneOf<CountryDto, IList<CountryDto>, None, ProblemDetails>> Handle(CountryQuery request, CancellationToken cancellationToken)
            {
                ArgumentGuard.NotNull(request, nameof(request));
                ArgumentGuard.NotNull(cancellationToken, nameof(cancellationToken));

                var validationResult = await Validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
                if (!validationResult.IsValid)
                {
                    return validationResult.ToProblemDetails();
                }

                // Handle request by CountryId if provided
                if (request.CountryId is not null)
                {
                    var country = await _repository.GetById(request.CountryId.Value, cancellationToken).ConfigureAwait(false);
                    return country is null ? new None() : _mapper.Map<CountryDto>(country);
                }

                // Handle request by CountryCode or return all countries
#pragma warning disable CS8604 // Possible null reference argument.
                var countries = !string.IsNullOrWhiteSpace(request.CountryCode)
                    ? new List<Country> { await _repository.GetByCountryCode(request.CountryCode, cancellationToken).ConfigureAwait(false) }
                    : await _repository.GetAll(cancellationToken).ConfigureAwait(false);
#pragma warning restore CS8604 // Possible null reference argument.

                // Return mapped list of countries or a default empty response
                return countries is not null ? _mapper.Map<List<CountryDto>>(countries) : new CountryDto();
            }
        }
    }
}
