#pragma warning disable CS8618

using Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using VendorConfiguration.Application.Dto;

namespace VendorConfiguration.Application.Commands
{
    /// <summary>
    /// Represents the command to modify an existing country.
    /// This command contains the data needed to update a country's details.
    /// </summary>
    public partial class ModifyCountryCommand
        : CommandBase, IRequest<OneOf<CountryDto, NotFound, ProblemDetails>>, IHybridBoundModel
    {
        /// <summary>
        /// Gets or sets the ID of the country to be modified.
        /// </summary>
        [HybridBindProperty(Source.Route)]
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the data that will be used to update the country.
        /// </summary>
        [HybridBindProperty(Source.Body)]
        public CountryRequest RequestData { get; set; }

        /// <summary>
        /// A collection of hybrid-bound properties that may be populated from various sources (e.g., route, query string, body).
        /// This property is used to define additional properties that are ignored in URI generation.
        /// </summary>
        [UriIgnore]
        public IDictionary<string, string> HybridBoundProperties => new Dictionary<string, string>();
    }
}
