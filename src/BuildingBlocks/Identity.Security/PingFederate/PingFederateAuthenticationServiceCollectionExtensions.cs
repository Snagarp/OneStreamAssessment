using System;
using Identity.Security.Abstract;
using Identity.Security.Attributes;
using Identity.Security.TokenValidators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Security.PingFederate
{
    /// <summary>
    /// Configures OAuth token validation component check if the request contains authorizated token.
    /// </summary>
    public static class PingFederateAuthenticationServiceCollectionExtensions
    {
        /// <summary>
        /// Registers OAuth token validation components.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors</param>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        public static void AddPingFederateAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration)
            {
                case null:
                    throw new ArgumentNullException(nameof(configuration));
            }

            services.AddSingleton<IJwtTokenHelper, JwtTokenHelper>();
            services.AddSingleton<ISecurityTokenValidator, PingIdentitySecurityTokenValidator>();
            services.AddScoped<PingFederateAuthAttribute>();

            services.ConfigureOptions<JwtBearerConfigureOptions>();
            services.ConfigureOptions<PingFederateAuthenticationConfigureOptions>();

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(); // Uses JwtBearerConfigureOptions [see above] for configuration
          
        }
    }
}
