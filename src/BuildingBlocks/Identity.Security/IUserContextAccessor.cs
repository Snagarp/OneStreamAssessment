using Identity.Security.Models;

namespace Identity.Security
{
    /// <summary>
    /// Defines a way to access current user context
    /// </summary>
    public interface IUserContextAccessor
    {
        /// <summary>Gets current user context.</summary>
        /// <returns></returns>
        JwtUserSetting UserContext { get; }

        /// <summary>Gets the user context specific key that can be used for caching.</summary>
        /// <value>The user context key.</value>
        string UserContextKey { get; }

        /// <summary>Builds the cache key using user Id and user context key.</summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        string BuildCacheKey(string prefix);
    }
}
