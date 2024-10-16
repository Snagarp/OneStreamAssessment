#pragma warning disable CS8618

using Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using VendorConfiguration.Application.Dto;

namespace VendorConfiguration.Application.Commands
{
    /// <summary>
    /// Command to create a new country. This command is hybrid-bound, meaning it binds properties from multiple sources such as headers, route, and body.
    /// </summary>
    public partial class CreateCountryCommand
        : CommandBase, IRequest<OneOf<CountryDto, ProblemDetails>>, IHybridBoundModel
    {
        /// <summary>
        /// Gets or sets the request data containing the details of the country to be created.
        /// </summary>
        [HybridBindProperty(Source.Body)]
        public CountryRequest RequestData { get; set; }

        /// <summary>
        /// Gets a dictionary of hybrid-bound properties that are excluded from URI parameters.
        /// </summary>
        [UriIgnore]
        public IDictionary<string, string> HybridBoundProperties => new Dictionary<string, string>();
    }

    /// <summary>
    /// Represents the data required to create a country.
    /// </summary>
    public class CountryRequest
    {
        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the ISO 3166-1 alpha-2 country code (2 characters).
        /// </summary>
        public string Iso3166CountryCode2 { get; set; }

        /// <summary>
        /// Gets or sets the ISO 3166-1 alpha-3 country code (3 characters).
        /// </summary>
        public string Iso3166CountryCode3 { get; set; }
    }
}
