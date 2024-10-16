using Microsoft.AspNetCore.Mvc;
using VendorConfiguration.Application.Commands;
using VendorConfiguration.Application.Dto;

namespace VendorConfiguration.Application.Queries
{
    /// <summary>
    /// Represents a query for retrieving country data. This query can filter by country ID or country code.
    /// </summary>
    public partial class CountryQuery
        : CommandBase, IRequest<OneOf<CountryDto, IList<CountryDto>, None, ProblemDetails>>, IHybridBoundModel
    {
        /// <summary>
        /// Gets or sets the optional country ID to filter the query results.
        /// </summary>
        [HybridBindProperty(Source.Route)]
        public int? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the optional country code to filter the query results.
        /// </summary>
        [HybridBindProperty(Source.QueryString)]
        public string? CountryCode { get; set; }

        /// <summary>
        /// Gets the collection of properties that are hybrid-bound from multiple sources, such as the route, query string, or body.
        /// </summary>
        public IDictionary<string, string> HybridBoundProperties => new Dictionary<string, string>();
    }
}
