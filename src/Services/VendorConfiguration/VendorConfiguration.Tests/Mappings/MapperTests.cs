using AutoMapper;
using VendorConfiguration.Api.Infrastructure.Mappers;

namespace VendorConfiguration.Tests.Mappings
{
    /// <summary>
    /// Contains unit tests to validate AutoMapper configuration for the <see cref="ConfigurationProfile"/>.
    /// </summary>
    public class MapperTests
    {
        /// <summary>
        /// Provides the AutoMapper configuration provider for testing.
        /// </summary>
        private readonly IConfigurationProvider _configurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapperTests"/> class.
        /// Configures AutoMapper using the <see cref="ConfigurationProfile"/>.
        /// </summary>
        public MapperTests()
        {
            _configurationProvider = new MapperConfiguration(cfg => cfg.AddProfile<ConfigurationProfile>());
        }

        /// <summary>
        /// Asserts that the AutoMapper configuration for <see cref="ConfigurationProfile"/> is valid.
        /// </summary>
        [Fact]
        public void ShouldBeNoErrorsWhenConfigurationIsValid() => _configurationProvider.AssertConfigurationIsValid();
    }
}
