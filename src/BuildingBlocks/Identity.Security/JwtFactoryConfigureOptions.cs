using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Security
{
    /// <summary>
    /// Handles the configuration of <see cref="JwtIssuerOptions"/> 
    /// </summary>
    /// <seealso cref="IConfigureOptions{JwtIssuerOptions}" />
    public class JwtFactoryConfigureOptions : IConfigureOptions<JwtIssuerOptions>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtFactoryConfigureOptions"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration used to retrieve JWT settings.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="configuration"/> is <c>null</c>.
        /// </exception>
        public JwtFactoryConfigureOptions(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Configures the <see cref="JwtIssuerOptions"/> instance with values from the configuration.
        /// </summary>
        /// <param name="options">The <see cref="JwtIssuerOptions"/> instance to configure.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="options"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the JWT secret key is not found or is empty in the configuration.
        /// </exception>
        public void Configure(JwtIssuerOptions options)
        {
            switch (options)
            {
                case null:
                    throw new ArgumentNullException(nameof(options));
            }

            // Retrieve the secret key from the configuration.
            var secret = _configuration["JwtIssuerOptions:Secret"];
            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new InvalidOperationException("JWT secret cannot be null or empty.");
            }

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            // Retrieve additional JWT options from the configuration section.
            var section = _configuration.GetSection(nameof(JwtIssuerOptions));

            options.Issuer = section[nameof(JwtIssuerOptions.Issuer)];
            options.Audience = section[nameof(JwtIssuerOptions.Audience)];
            options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
