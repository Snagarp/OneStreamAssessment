using Microsoft.AspNetCore.Mvc;
using VendorConfiguration.Application.Queries;
using Web.VendorConfiguration.HttpAggregator.Services;

namespace Web.VendorConfiguration.HttpAggregator.Controllers
{
    /// <summary>
    /// Handles API requests related to vendor configurations.
    /// </summary>
    public class VendorConfigurationController : BaseApiController
    {
        private readonly IVendorConfigurationService _vendorConfigurationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorConfigurationController"/> class.
        /// </summary>
        /// <param name="vendorConfigurationService">The vendor configuration service.</param>
        public VendorConfigurationController(IVendorConfigurationService vendorConfigurationService) =>
            _vendorConfigurationService = ArgumentGuard.NotNull(vendorConfigurationService);

        /// <summary>
        /// Retrieves the list of countries based on the provided query parameters.
        /// </summary>
        /// <param name="query">The query parameters for retrieving countries.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of countries.</returns>
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries(CountryQuery query)
        {
            var response = await _vendorConfigurationService.GetCountries(query).ConfigureAwait(false);
            return await ConvertToActionResult(response).ConfigureAwait(false);
        }       
    }
}
