using Common.Attributes;
using Common.Configuration;
using Common.Filters;
using Common.Http;
using Identity.Security.PingFederate;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Common.Handler;
using HybridModelBinding;
using Web.VendorConfiguration.HttpAggregator.Config;

namespace Web.VendorConfiguration.HttpAggregator.Services
{
    /// <summary>
    /// Extension methods to add custom services and configurations for the Vendor Configuration Aggregator.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures MVC settings, including custom filters, JSON options, CORS policy, and API behavior options.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddGatewayCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter)); // Error handling filter
                options.Filters.Add(new BenchmarkAttribute()); // Adds elapsed time header to responses
            })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddHybridModelBinder();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });

            services.Configure<UrlConfig>(configuration.GetSection("urls"));
            services.ConfigureSwagger("v1", "Vendor Configuration Aggregator for Web Clients", "Vendor Configuration Aggregator for Web Clients");

            return services;
        }

        /// <summary>
        /// Adds application-specific services, including common integrations and authentication.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddGatewayApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            services.AddCommonIntegrations(configuration);
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpContextAccessor();
            services.Configure<UrlConfig>(configuration.GetSection("urls"));
            services.AddPingFederateAuthentication(configuration);

            return services;
        }

        /// <summary>
        /// Adds custom integrations for the Vendor Configuration service, including HTTP client configuration and logging.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddGatewayCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            var section = configuration.GetSection("urls");
            var fromCfg = section.GetValue<string>("VendorConfiguration");
            services.AddTransient<HttpClientLoggingHandler>();
            services.AddScoped<IVendorConfigurationService, VendorConfigurationService>();
            services.AddHttpClient<IVendorConfigurationService, VendorConfigurationService>(client =>
            {
                client.BaseAddress = new Uri(fromCfg!);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddHttpMessageHandler<HttpClientLoggingHandler>();

            return services;
        }
    }
}
