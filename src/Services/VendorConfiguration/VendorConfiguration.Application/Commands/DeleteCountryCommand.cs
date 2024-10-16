using Common.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace VendorConfiguration.Application.Commands
{
    /// <summary>
    /// Command to delete an existing country. This command is hybrid-bound, meaning it binds properties from multiple sources such as headers, route, and body.
    /// </summary>
    public partial class DeleteCountryCommand
        : CommandBase, IRequest<OneOf<Success, OneOf.Types.NotFound, ProblemDetails>>, IHybridBoundModel
    {
        [HybridBindProperty(Source.Route)]
        public int CountryId { get; set; }

        [UriIgnore]
        public IDictionary<string, string> HybridBoundProperties => new Dictionary<string, string>();
    }
}
