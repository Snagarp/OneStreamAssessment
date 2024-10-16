using Identity.Security.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Security.PingFederate
{
    /// <summary>
    /// Extensions to register services for OAuth authorization like:
    /// - AuthorizingOAuthScopesFilter for MVC filter
    /// - AuthorizingOAuthScopesAttribute
    /// </summary>
    public static class PingFederateAuthenticationServiceCollectionExtensionsForMvc
    {
        /// <summary>
        /// Will add oauth filter to MVC.
        /// </summary>
        public static void AddAuthorizingOAuthScopesFilterForMvc(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationFilter, AuthorizingOAuthScopesFilter>();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(AuthorizingOAuthScopesFilter));
            });
        }
    }
}
