
namespace VendorConfiguration.Api.Configuration
{
    public record ApiConfiguration
    {
        required public string ApiName { get; set; }
        public string? BaseUrl { get; set; }
        public string? SandboxBaseUrl { get; set; }
        public string? ConfigurationFile { get; set; }

    }
}
