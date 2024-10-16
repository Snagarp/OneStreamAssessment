using System;
using Identity.Security.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Identity.Security
{
    /// <summary>
    ///     Bakes security cookies
    /// </summary>
    public sealed class CookieBaker : ICookieBaker
    {
        private const string _DEFAULT_DOMAIN = "s1nextgen.com";
        private const int _COOKIE_EXPIRY_TTIME = 720;
        private const string _KEY_JWT = "JwtToken";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cookieDomain;
        private readonly bool _isHttpOnly;

        /// <summary>Initializes a new instance of the <see cref="CookieBaker" /> class.</summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <exception cref="System.ArgumentNullException">httpContextAccessor</exception>
        public CookieBaker(IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            if (environment.IsDevelopment())
            {
                _cookieDomain = string.Empty;
                _isHttpOnly = false;
            }
            else
            {
                _cookieDomain = configuration.GetValue("JwtIssuerOptions:CookieDomain", _DEFAULT_DOMAIN);
                _isHttpOnly = configuration.GetValue("JwtIssuerOptions:IsHttpOnlySecureCookie", true);
            }

        }

        #region ICookieBaker Members

        /// <summary>Adds JWT cookies to response.</summary>
        /// <param name="jwtToken">The JWT token.</param>
        public void AddJwtCookie(string jwtToken)
        {

            var options = BuildOptions(TimeSpan.FromMinutes(_COOKIE_EXPIRY_TTIME));

            GetResponseCookies().Append(_KEY_JWT, jwtToken, options);
        }

        /// <summary>Adds the anti-forgery cookie to current HTTP response.</summary>
        public void AddAntiForgeryCookie()
        {
            //var tokens = _antiForgery.GetAndStoreTokens(_httpContextAccessor.HttpContext);
            //var options = new CookieOptions { HttpOnly = false, Domain = _cookieDomain };
            //GetResponseCookies().Append(_KEY_XSRF_TOKEN, tokens.RequestToken, options);
        }

        /// <summary>Removes JWT cookies from response.</summary>
        public void RemoveJwtCookie()
        {
            var options = BuildOptions(TimeSpan.FromDays(-1));

            var cookies = GetResponseCookies();

            cookies.Append(_KEY_JWT, string.Empty, options);
        }

        #endregion

        private CookieOptions BuildOptions(TimeSpan expiration) => new()
        {
            Domain = _cookieDomain,
            Secure = _isHttpOnly,
            HttpOnly = _isHttpOnly,
            IsEssential = true,
            Expires = DateTime.UtcNow.Add(expiration),
            SameSite = SameSiteMode.None
        };

        private IResponseCookies GetResponseCookies()
        {
            var cookies = _httpContextAccessor.HttpContext?.Response?.Cookies;

            return cookies switch
            {
                null => throw new InvalidOperationException(
                    "Cannot access response cookies collection on current HTTP context."),
                _ => cookies
            };
        }
    }
}
