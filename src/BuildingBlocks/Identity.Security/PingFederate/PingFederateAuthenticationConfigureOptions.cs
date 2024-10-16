using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Identity.Security.PingFederate
{
    /// <summary>
    /// Configures <see cref="PingFederateAuthenticationOptions"/>.
    /// </summary>
    /// <seealso cref="IConfigureOptions{PingFederateAuthenticationOptions}" />
    public sealed class PingFederateAuthenticationConfigureOptions : IConfigureOptions<PingFederateAuthenticationOptions>
    {
        private const string _PREFIX = "PingFederateAuthentication:";
        private readonly IConfiguration _configuration;

        /// <summary>Initializes a new instance of the <see cref="PingFederateAuthenticationConfigureOptions" /> class.</summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        public PingFederateAuthenticationConfigureOptions(
            IConfiguration configuration
        )
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Invoked to configure a <see cref="PingFederateAuthenticationOptions"/> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(PingFederateAuthenticationOptions options)
        {
            switch (options)
            {
                case null:
                    throw new ArgumentNullException(nameof(options));
            }

            options.UseDistributedCache = _configuration.GetValue<bool>(_PREFIX + "UseDistributedCache");
            options.UseDataProtection = _configuration.GetValue<bool>(_PREFIX + "UseDataProtection");
            options.CachedTokenExpirationTimeInMinutes = _configuration.GetValue<int>(_PREFIX + "CachedTokenExpirationTimeInMinutes");

            options.DefaultEndpoint = new PingEndpointSettings
            {
                ClientId = _configuration.GetValue<string>(_PREFIX + "USServiceClientId"),
                ClientSecret = _configuration.GetValue<string>(_PREFIX + "USServiceClientSecret"),
                Authority = _configuration.GetValue<string>(_PREFIX + "USServiceAuthority")
            };

            foreach (var pair in options.Endpoints)
            {
                if (string.IsNullOrEmpty(pair.Value.Authority))
                {
                    throw new InvalidOperationException($"Configuration entry '{_PREFIX}ServiceAuthority' is either missing or empty.");
                }
                if (string.IsNullOrEmpty(pair.Value.ClientId))
                {
                    throw new InvalidOperationException($"Configuration entry '{_PREFIX}ClientId' is either missing or empty.");
                }
                if (string.IsNullOrEmpty(pair.Value.ClientSecret))
                {
                    throw new InvalidOperationException($"Configuration entry '{_PREFIX}ClientSecret' is either missing or empty.");
                }
            }
        }
    }
}
