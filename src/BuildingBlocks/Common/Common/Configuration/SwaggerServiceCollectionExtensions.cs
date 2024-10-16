using Common.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.Configuration
{
    /// <summary>
    /// Extension methods for configuring Swagger in the service collection.
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        /// <summary>
        /// Configures Swagger with the specified version, title, and description for the API documentation.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="version">The version of the API.</param>
        /// <param name="title">The title of the API.</param>
        /// <param name="description">A brief description of the API.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with Swagger configured.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="version"/>, <paramref name="title"/>, or <paramref name="description"/> is null.
        /// </exception>
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, string version, string title, string description)
        {
            switch (version)
            {
                case null:
                    throw new ArgumentNullException(nameof(version));
            }
            switch (title)
            {
                case null:
                    throw new ArgumentNullException(nameof(title));
            }
            switch (description)
            {
                case null:
                    throw new ArgumentNullException(nameof(description));
                default:
                    // Add Swagger generation with basic configuration
                    services.AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title = title,
                            Version = version,
                            Description = description
                        });

                        // Apply a custom operation filter for authorization checks in Swagger UI
                        options.OperationFilter<AuthorizeCheckOperationFilter>();
                    });

                    return services;
            }
        }
    }
}
