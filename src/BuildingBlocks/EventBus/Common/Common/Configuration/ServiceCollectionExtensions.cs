//2023 (c) TD Synnex - All Rights Reserved.

using Common.Attributes;
using Common.Filters.HttpGlobalExceptionFilter;
using Common.Utils;
using Common.Validation;

using Identity.Security.OAuth;
using Identity.Security.OAuth.Abstract;
using Identity.Security.OAuth.PingFederate;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Text.Json.Serialization;

namespace Common.Configuration
{
    public static class CommonServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICookieBaker, CookieBaker>();
            services.AddJwtFactory();
            services.AddDistributedMemoryCache();
            services.AddAuthorizingOAuthScopesFilterForMvc();
            services.AddSingleton<IModelUriBuilder, ModelUriBuilder>();

            return services;
        }

        public static IServiceCollection AddApiBehaviorConfiguration(this IServiceCollection services, IConfiguration configuration)
        { 
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

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
 
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            services.AddApplicationInsightsKubernetesEnricher();
            return services;
        }

        public static IServiceCollection AddCommonMvc(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

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
