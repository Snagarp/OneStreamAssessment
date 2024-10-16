using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Security.PingFederate
{
    /// <summary>
    /// Configures <see cref="JwtBearerOptions"/>
    /// </summary>
    /// <seealso cref="IConfigureOptions{JwtBearerOptions}" />
    public class JwtBearerConfigureOptions : IConfigureOptions<JwtBearerOptions>
    {
        private readonly ISecurityTokenValidator _validator;

        /// <summary>Initializes a new instance of the <see cref="JwtBearerConfigureOptions" /> class.</summary>
        /// <param name="validator">The validator.</param>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        public JwtBearerConfigureOptions(ISecurityTokenValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Invoked to configure <see cref="JwtBearerOptions"/> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Configure(JwtBearerOptions options)
        {
            switch (options)
            {
                case null:
                    throw new ArgumentNullException(nameof(options));
            }

            options.SecurityTokenValidators.Clear();
            options.SecurityTokenValidators.Add(_validator);
            options.TokenValidationParameters = new TokenValidationParameters { SaveSigninToken = true };
        }
    }
}
