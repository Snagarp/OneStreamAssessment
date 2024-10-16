namespace VendorConfiguration.Application.Commands
{
    /// <summary>
    /// Represents the base command class that captures essential metadata and contextual information
    /// such as user details, IP address, correlation ID, and other common fields.
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// Gets or sets the username of the user executing the command.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the IP address from which the request originated.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the correlation ID to uniquely track the request flow across multiple services.
        /// This value is typically passed via the HTTP header 'x-correlation-id'.
        /// </summary>
        [HybridBindProperty(Source.Header, "x-correlation-id")]
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the request ID, typically used to track individual requests.
        /// This value is passed via the HTTP header 'x-request-id'.
        /// </summary>
        [HybridBindProperty(Source.Header, "x-request-id")]
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets or sets the region key, which identifies a specific region.
        /// The value is extracted from the route, query string, or body of the request.
        /// </summary>
        [HybridBindProperty(new[] { Source.Route, Source.QueryString, Source.Body })]
        public string? RegionKey { get; set; }

        /// <summary>
        /// Gets or sets the vendor key, which identifies the vendor making the request.
        /// The value is extracted from the route or query string.
        /// </summary>
        [HybridBindProperty(new[] { Source.Route, Source.QueryString })]
        public string? VendorKey { get; set; }
    }
}
