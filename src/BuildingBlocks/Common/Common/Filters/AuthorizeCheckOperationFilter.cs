using Common.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
#pragma warning disable CA1861

namespace Common.Filters
{
    /// <summary>
    /// A Swagger operation filter that checks for the presence of the <see cref="AuthorizeAttribute"/> and adds security definitions to the API documentation.
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the Swagger operation by adding security requirements if the operation or its containing type is decorated with the <see cref="AuthorizeAttribute"/>.
        /// </summary>
        /// <param name="operation">The Swagger <see cref="OpenApiOperation"/> to modify.</param>
        /// <param name="context">The <see cref="OperationFilterContext"/> that provides additional context for the filter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="operation"/> or <paramref name="context"/> is <c>null</c>.</exception>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ArgumentGuard.NotNull(operation, nameof(operation));
            ArgumentGuard.NotNull(context, nameof(context));

            // Check for the presence of the Authorize attribute on the method or the containing type.
            bool hasAuthorize = context!.MethodInfo!.DeclaringType!.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            switch (hasAuthorize)
            {
                case false:
                    return;
            }

            // Add Unauthorized and Forbidden responses.
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            // Define the OAuth2 security scheme.
            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };

            // Add security requirements to the operation.
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [ oAuthScheme ] = new[] { "OneStream.Integrations.Web.VendorConfiguration.HttpAggregator" }
                }
            };
        }
    }
}
