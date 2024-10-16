//2023 (c) TD Synnex - All Rights Reserved.



using Common.Filters;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.Configuration
{
    public static class SwaggerServiceCollectionExtensions    {

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, string version, string title, string description)
        {
            if (version is null) throw new ArgumentNullException(nameof(version));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (description is null) throw new ArgumentNullException(nameof(description));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = title,
                    Version = version,
                    Description = description
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }
    }
}
