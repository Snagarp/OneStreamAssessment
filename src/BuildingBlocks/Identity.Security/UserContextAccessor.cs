using System;
using Identity.Security.Abstract;
using Identity.Security.Models;
using Microsoft.AspNetCore.Http;

namespace Identity.Security
{
    /// <summary>
    ///     Provides access to current User Context
    /// </summary>
    public sealed class UserContextAccessor : IUserContextAccessor
    {
        private const string _USER_CONTEXT = "UserContext";
        private const string _USER_CONTEXT_KEY = "UserContextKey";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenHelper _tokenParser;

        /// <summary>Initializes a new instance of the <see cref="UserContextAccessor" /> class.</summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="tokenParser">The JWT parser.</param>
        /// <exception cref="ArgumentNullException">httpContextAccessor</exception>
        public UserContextAccessor(IHttpContextAccessor httpContextAccessor, IJwtTokenHelper tokenParser)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _tokenParser = tokenParser ?? throw new ArgumentNullException(nameof(tokenParser));
        }

        #region IUserContextAccessor Members

        /// <summary>Gets current user context.</summary>
        /// <returns></returns>
        public JwtUserSetting UserContext
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;

                switch (context)
                {
                    // When called from Azure function, the context is null
                    case null:
                        return new JwtUserSetting();
                }

                if (context.Items.TryGetValue(_USER_CONTEXT, out var itemValue))
                {
                    return (JwtUserSetting)itemValue;
                }

                JwtUserSetting userContext = null;

                if (context.Request != null)
                {
                    var token = _tokenParser.GetJwtTokenFromHeaderOrCookie(context.Request);

                    switch (string.IsNullOrEmpty(token))
                    {
                        case false:
                            userContext = _tokenParser.GetUserJwtSetting(token);
                            break;
                    }
                }

                switch (userContext)
                {
                    case null:
                        userContext = new JwtUserSetting();
                        break;
                }

                context.Items.Add(_USER_CONTEXT, userContext);

                return userContext;
            }
        }

        /// <summary>Gets the user context specific key that can be used for caching.</summary>
        /// <value>The user context key.</value>
        public string UserContextKey
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;

                if (context.Items.TryGetValue(_USER_CONTEXT_KEY, out var keyValue))
                {
                    return (string)keyValue;
                }

                string key = null;

                if (context.Request != null)
                {
                    var token = _tokenParser.GetJwtTokenFromHeaderOrCookie(context.Request);

                    switch (string.IsNullOrEmpty(token))
                    {
                        case false:
                            key = token[^15..];
                            context.Items.Add(_USER_CONTEXT_KEY, key);
                            break;
                    }
                }

                return key;
            }
        }

        /// <summary>Builds the cache key using user Id and user context key.</summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">prefix</exception>
        public string BuildCacheKey(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            return $"{prefix}_{UserContext.UserId}_{UserContextKey}";
        }

        #endregion
    }
}
