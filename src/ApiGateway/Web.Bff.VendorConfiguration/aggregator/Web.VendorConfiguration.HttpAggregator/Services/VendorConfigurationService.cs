using Common.Utils;
using VendorConfiguration.Application.Queries;
using Web.VendorConfiguration.HttpAggregator.Config;

namespace Web.VendorConfiguration.HttpAggregator.Services
{
    /// <summary>
    /// Service responsible for handling vendor configuration operations, including fetching countries and regions.
    /// </summary>
    public class VendorConfigurationService : IVendorConfigurationService
    {
        private readonly HttpClient _httpClient;
        private readonly IModelUriBuilder _uriBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorConfigurationService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for making requests.</param>
        /// <param name="builder">The URI builder for generating API endpoints.</param>
        public VendorConfigurationService(HttpClient httpClient, IModelUriBuilder builder)
        {
            _httpClient = ArgumentGuard.NotNull(httpClient);
            _uriBuilder = ArgumentGuard.NotNull(builder);
        }

        /// <summary>
        /// Retrieves a list of countries based on the provided query parameters.
        /// </summary>
        /// <param name="query">The query parameters for retrieving countries.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> containing the list of countries.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the query parameter is null.</exception>
        public async Task<HttpResponseMessage> GetCountries(CountryQuery query)
        {
            ArgumentGuard.NotNull(query, nameof(query));

            var uri = _uriBuilder.Build(_httpClient, CountryOperations.GetCountry, query);
            var result = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            return result;
        }   
    }
}
