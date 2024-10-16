using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.VendorConfiguration.HttpAggregator.Filters
{
    /// <summary>
    /// Operation filter that checks for the presence of the <see cref="AuthorizeAttribute"/> and adds security requirements to the Swagger operation.
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private static readonly string[] item = new[] { "Web.VendorConfiguration.HttpAggregator" };

        /// <summary>
        /// Applies the filter to the Swagger operation to ensure that the operation includes the appropriate security requirements.
        /// </summary>
        /// <param name="operation">The <see cref="OpenApiOperation"/> representing the Swagger operation.</param>
        /// <param name="context">The <see cref="OperationFilterContext"/> providing context about the operation.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ArgumentGuard.NotNull(operation, nameof(operation));
            ArgumentGuard.NotNull(context, nameof(context));

            // Check if the operation or its declaring type has an Authorize attribute
            var hasAuthorize = context!.MethodInfo!.DeclaringType!.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            // If no Authorize attribute is found, exit the method
            if (!hasAuthorize)
            {
                return;
            }

            // Add 401 Unauthorized and 403 Forbidden response codes to the operation
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            // Define the OAuth2 security scheme
            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };

            // Add security requirements to the operation
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new() {
                    [ oAuthScheme ] = item
                }
            };
        }
    }
}
