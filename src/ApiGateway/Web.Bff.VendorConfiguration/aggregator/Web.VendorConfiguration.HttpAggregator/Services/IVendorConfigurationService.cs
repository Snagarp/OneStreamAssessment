using VendorConfiguration.Application.Queries;

namespace Web.VendorConfiguration.HttpAggregator.Services
{
    /// <summary>
    /// Service interface for handling vendor configuration related operations.
    /// </summary>
    public interface IVendorConfigurationService
    {
        /// <summary>
        /// Retrieves the list of countries based on the provided query parameters.
        /// </summary>
        /// <param name="query">The <see cref="CountryQuery"/> containing parameters for filtering the countries.</param>
        /// <returns>A <see cref="Task{HttpResponseMessage}"/> representing the asynchronous operation that contains the HTTP response message.</returns>
        Task<HttpResponseMessage> GetCountries(CountryQuery query);
    }
}
