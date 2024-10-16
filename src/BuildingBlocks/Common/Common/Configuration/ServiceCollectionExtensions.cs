using Common.Attributes;
using Common.Utils;
using Common.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Common.Filters;
using Identity.Security;
using Identity.Security.Abstract;
using Identity.Security.PingFederate;

namespace Common.Configuration
{
    /// <summary>
    /// Extension methods for adding common services and configurations to the application.
    /// </summary>
    public static class CommonServiceCollectionExtensions
    {
        /// <summary>
        /// Adds common integrations to the application's service collection.
        /// </summary>
        /// <param name="services">The service collection to add the integrations to.</param>
        /// <param name="configuration">The application's configuration settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with added integrations.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
        public static IServiceCollection AddCommonIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICookieBaker, CookieBaker>();
            services.AddDistributedMemoryCache();
            services.AddAuthorizingOAuthScopesFilterForMvc();
            services.AddSingleton<IModelUriBuilder, ModelUriBuilder>();

            return services;
        }

        /// <summary>
        /// Configures API behavior, particularly for handling model validation errors.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The application's configuration settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with API behavior configured.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
        public static IServiceCollection AddApiBehaviorConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration)
            {
                case null:
                    throw new ArgumentNullException(nameof(configuration));
                default:
                    services.Configure<ApiBehaviorOptions>(options =>
                    {
                        options.InvalidModelStateResponseFactory = context =>
                        {
                            var problemDetails = new ValidationProblemDetails(context.ModelState)
                            {
                                Instance = context.HttpContext.Request.Path,
                                Status = StatusCodes.Status400BadRequest,
                                Detail = "Please refer to the errors property for additional details."
                            };

                            return new BadRequestObjectResult(problemDetails)
                            {
                                ContentTypes = { "application/problem+json", "application/problem+xml" }
                            };
                        };
                    });

                    return services;
            }
        }

        /// <summary>
        /// Adds Application Insights telemetry and Kubernetes enricher to the service collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The application's configuration settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with Application Insights added.</returns>
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            services.AddApplicationInsightsKubernetesEnricher();
            return services;
        }

        /// <summary>
        /// Adds common MVC configuration, including JSON and XML serializers, error handling, and CORS policy.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The application's configuration settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with common MVC configuration.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
        public static IServiceCollection AddCommonMvc(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration)
            {
                case null:
                    throw new ArgumentNullException(nameof(configuration));
            }

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter)); // Error Handling
                options.Filters.Add(new BenchmarkAttribute());          // Adds elapsed time header to responses
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddXmlSerializerFormatters();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }
    }
}