using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Identity.Security.Abstract;
using Identity.Security.Enums;
using Identity.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Identity.Security.Filters
{
    /// <summary>
    /// Checks if HTTP request is authorized
    /// </summary>
    /// <seealso cref="IAuthorizationFilter" />
    public class AuthorizingOAuthScopesFilter : IAuthorizationFilter
    {
        private readonly ILogger _logger;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly bool _enableCustomLogin;
        
        private readonly string _secret;

        /// <summary>Initializes a new instance of the <see cref="AuthorizingOAuthScopesFilter"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="jwtTokenHelper">The JWT token helper.</param>
        public AuthorizingOAuthScopesFilter(
            ILogger<AuthorizingOAuthScopesFilter> logger,
            IConfiguration configuration,
            IJwtTokenHelper jwtTokenHelper = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtTokenHelper = jwtTokenHelper ?? new JwtTokenHelper();

            switch (configuration)
            {
                case null:
                    throw new ArgumentNullException(nameof(configuration));
            }
            var disableCustomLogin = configuration["JwtIssuerOptions:DisableCustomLogin"];
                    _secret = configuration["JwtIssuerOptions:secret"];   
            _enableCustomLogin = disableCustomLogin == null || disableCustomLogin == "no";
            if (_secret != null)
                _logger.LogInformation("Secret: {Secret}", _secret);
        }

        #region IAuthorizationFilter Members

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The <see cref="AuthorizationFilterContext" />.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region Argument Validation

            switch (context)
            {
                case null:
                    throw new ArgumentNullException(nameof(context));
            }

            var request = context.HttpContext?.Request;
            switch (request)
            {
                case null:
                    throw new ArgumentException("HTTP request is not set", nameof(context));
            }

            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                return;
            }

            if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
            {
                return;
            }

            #endregion

            try
            {
                var methodAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true);

                if (IsAuthorizationBypassed(methodAttributes))
                {
                    return;
                }

                var token = _jwtTokenHelper.GetJwtTokenFromHeaderOrCookie(request);


                if (!ValidateJwtTokenAsync(request.Path.Value, token).GetAwaiter().GetResult())
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Get the list of scopes provided in the OAuth token.
                var identity = (ClaimsIdentity)context.HttpContext.User.Identity;
                switch (identity)
                {
                    case null:
                        return;
                }
                var claims = identity.Claims;
                var scopeClaim = claims
                    .FirstOrDefault(c => c.Type == "OAuthScope" && c.ValueType == ClaimValueTypes.String);

                string[] identityScopes = null;
                if (scopeClaim != null && !string.IsNullOrEmpty(scopeClaim.Value))
                {
                    identityScopes = scopeClaim.Value.Trim().Split(' ');
                }

                // Gather information for logging.
                var actionName = controllerActionDescriptor.ActionName;
                var controllerName = controllerActionDescriptor.ControllerName;

                var controllerAttributes = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(true);
                if (!CheckIdentityScopes(controllerAttributes, identityScopes))
                {
                    var scopes = GetClaimValue(scopeClaim);

                    _logger.LogInformation(
                       "{ControllerName}.{ActionName}: Action OAuth scope requirements not met. Current context has the following scopes: {Scopes}",
                       controllerName, actionName, scopes);

                    context.Result = new UnauthorizedResult();
                    return;
                }

                if (!CheckIdentityScopes(methodAttributes, identityScopes))
                {
                    var scopes = GetClaimValue(scopeClaim);

                    _logger.LogInformation(
                     "{ControllerName}.{ActionName}: Action OAuth scope requirements not met. Current context has the following scopes: {Scopes}",
                     controllerName, actionName, scopes);

                    context.Result = new UnauthorizedResult();
                    return;
                }

                var userContext = _jwtTokenHelper.GetUserJwtSetting(token) ?? new JwtUserSetting();

                var permissions = userContext.GetPid();

                if (!CheckUserPermissions(methodAttributes, permissions))
                {

                    var requiredPermissions = GetRequiredPermissionsForMethod(methodAttributes);

                    _logger.LogInformation(
                    "{ControllerName}.{ActionName}: Action permissions requirements not met. Current context has the following permissions: {RequiredPermissions}",
                    controllerName, actionName, requiredPermissions);

                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authorization: {ErrorMessage}", ex.Message);
                context.Result = new UnauthorizedResult();
            }
        }

        #endregion

        private static string GetClaimValue(Claim claim) => claim == null
                ? "<null>"
                : string.IsNullOrEmpty(claim.Value)
                    ? "<empty>"
                    : claim.Value;

        private static bool IsAuthorizationBypassed(object[] methodAttributes)
        {
            if (methodAttributes.OfType<ServiceFilterAttribute>()
                .Any(x => string.CompareOrdinal(x.ServiceType.Name, "PingFederateAuthAttribute") == 0))
            {
                return true;
            }

            return false;
        }

        private Task<bool> ValidateJwtTokenAsync(string requestPath, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(false);
            }

            if (string.IsNullOrEmpty(requestPath))
            {
                return Task.FromResult(false);
            }

            _ = CreateEncodedMessageHash(token, _secret);

            if (!_jwtTokenHelper.ValidateJwtToken(token, _secret))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private bool CheckIdentityScopes(object[] attributes, string[] identityScopes)
        {
            switch (attributes.Length)
            {
                case 0:
                    return true;
            }

            var scopeAttributes = attributes
                .OfType<AuthorizingOAuthScopesAttribute>()
                .ToArray();

            switch (scopeAttributes.Length)
            {
                case 0:
                    return true;
            }

            if (identityScopes == null || identityScopes.Length == 0)
            {
                // there are no oAuth scopes when custom auth is used.
                return _enableCustomLogin;
            }

            foreach (var s1 in identityScopes)
            {
                foreach (var a in scopeAttributes)
                {
                    foreach (var s2 in a.Scopes)
                    {
                        switch (string.CompareOrdinal(s1, s2))
                        {
                            case 0:
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        private static string GetRequiredPermissionsForMethod(object[] attributes) => string.Join(" , ",
                (from a
                    in attributes.OfType<PermissionAttribute>()
                 from p in a.Permissions
                 select p).Distinct());

        private static bool CheckUserPermissions(object[] attributes, List<int> permissionsList)
        {
            switch (attributes.Length)
            {
                case 0:
                    return true;
            }

            var permissionAttributes = attributes.OfType<PermissionAttribute>().ToArray();

            switch (permissionAttributes.Length)
            {
                case 0:
                    return true;
            }

            if (permissionsList == null || permissionsList.Count == 0)
            {
                return false;
            }

            if (permissionsList.Contains((int)PermissionsEnum.Permissions.OneStreamAdmin))
            {
                return true;
            }

            foreach (var p1 in permissionsList)
            {
                foreach (var a in permissionAttributes)
                {
                    foreach (var p2 in a.Permissions)
                    {
                        if (p1 == p2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static string CreateEncodedMessageHash(string message, string secret)
        {
            var encoding = new UTF8Encoding();
            var keyBytes = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(message);
#pragma warning disable CA5350 // Do Not Use Weak Cryptographic Algorithms
            using var algorithm = new HMACSHA1(keyBytes);
#pragma warning restore CA5350 // Do Not Use Weak Cryptographic Algorithms
            var hashMessage = algorithm.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashMessage);
        }
    }
}
