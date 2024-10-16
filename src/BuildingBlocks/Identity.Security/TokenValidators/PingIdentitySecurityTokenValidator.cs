using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Identity.Security.Abstract;
using Identity.Security.Models;
using Identity.Security.PingFederate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("Identity.Security.OAuth.Test")]

namespace Identity.Security.TokenValidators
{
    /// <summary>
    /// Validates security tokens using Ping Federate.
    /// </summary>
    public sealed class PingIdentitySecurityTokenValidator : ISecurityTokenValidator
    {
        private readonly ILogger<PingIdentitySecurityTokenValidator> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly IOptions<PingFederateAuthenticationOptions> _options;
     
        /// <summary>
        /// Initializes a new instance of the <see cref="PingIdentitySecurityTokenValidator"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging operations and errors.</param>
        /// <param name="options">Ping Federate authentication options.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="jwtTokenHelper">A helper for working with JWT tokens.</param>
        public PingIdentitySecurityTokenValidator(
            ILogger<PingIdentitySecurityTokenValidator> logger,
            IOptions<PingFederateAuthenticationOptions> options,
            IHttpContextAccessor httpContextAccessor,
            IJwtTokenHelper jwtTokenHelper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _jwtTokenHelper = jwtTokenHelper ?? throw new ArgumentNullException(nameof(jwtTokenHelper));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            MaximumTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
        }

        #region ISecurityTokenValidator Members

        /// <summary>
        /// Gets a value indicating whether the validator can validate tokens.
        /// </summary>
        public bool CanValidateToken => true;

        /// <summary>
        /// Gets or sets the maximum size in bytes for tokens to be processed.
        /// </summary>
        public int MaximumTokenSizeInBytes { get; set; }

        /// <summary>
        /// Determines whether the specified token can be read.
        /// </summary>
        /// <param name="securityToken">The token to validate.</param>
        /// <returns><c>true</c> if the token can be read; otherwise, <c>false</c>.</returns>
        public bool CanReadToken(string securityToken) => !string.IsNullOrEmpty(securityToken);

        /// <summary>
        /// Validates a security token.
        /// </summary>
        /// <param name="securityToken">The security token to validate.</param>
        /// <param name="validationParameters">The token validation parameters.</param>
        /// <param name="validatedToken">The validated token.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> representing the validated user.</returns>
        /// <exception cref="SecurityTokenException">Thrown if the token validation fails.</exception>
        public ClaimsPrincipal ValidateToken(
            string securityToken,
            TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            _logger.LogDebug("Validating token.");

            var settings = _options.Value;
            var endpoint = settings.UseDefaultEndpoint ? settings.DefaultEndpoint : GetPingEndpoint();

            var result = GetFromCache(securityToken);
            switch (result.IsValid)
            {
                case false:
                {
                    _logger.LogInformation("Retrieving security token from Ping Federate.");
                    result = Validate(securityToken, endpoint).Result;

                    switch (result.IsValid)
                    {
                        case true:
                            SetToCache(securityToken, result, endpoint.ClientId);
                            break;
                    }

                    break;
                }
            }

            switch (result.IsValid)
            {
                case true:
                {
                    var identity = new GenericIdentity(result.SecurityToken.Username);
                    identity.AddClaim(new Claim("OAuthScope", result.SecurityToken.Scope ?? string.Empty));
                    identity.AddClaim(new Claim("name", result.SecurityToken.Username ?? string.Empty));
                    validatedToken = result.SecurityToken;
                    return new ClaimsPrincipal(identity);
                }
                default:
                    throw new SecurityTokenException("OAuth authorization failed.");
            }
        }

        /// <summary>
        /// Represents the response of a security token validation operation.
        /// </summary>
        private class ValidateResponse
        {
            /// <summary>
            /// Gets or sets a value indicating whether the token is valid.
            /// </summary>
            public bool IsValid { get; set; }

            /// <summary>
            /// Gets or sets the validated <see cref="PingSecurityToken"/> instance.
            /// </summary>
            public PingSecurityToken SecurityToken { get; set; }
        }

        #endregion

        private PingEndpointSettings GetPingEndpoint()
        {
            var region = _httpContextAccessor.HttpContext != null &&
                         _jwtTokenHelper.GetPingRegion(_httpContextAccessor.HttpContext.Request) == "EU"
                ? "EU"
                : "US";

            if (!_options.Value.Endpoints.TryGetValue(region, out var endpoint))
            {
                _logger.LogError("Missing Ping configuration for {Region} endpoint.", region);
                throw new SecurityTokenException("OAuth authorization failed.");
            }

            return endpoint;
        }

        private void SetToCache(string securityToken, ValidateResponse result, string clientId) => _logger.LogDebug("Saving security token to the cache.");// TODO: Implement cache logic.

        private ValidateResponse GetFromCache(string securityToken)
        {
            if (securityToken is not null)
            {
                _logger.LogDebug("Retrieving security token from the cache.");
                // TODO: Implement cache retrieval logic.
                return new ValidateResponse { IsValid = false };
            }

            throw new ArgumentNullException(nameof(securityToken));
        }

        private async Task<ValidateResponse> Validate(string oAuthToken, PingEndpointSettings endpoint)
        {
            var request = new Dictionary<string, string> { { "token", oAuthToken } };
            var result = new ValidateResponse();

            try
            {
                await PostValidateTokenRequestToPing(request, result, endpoint);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation failed with exception: {ExceptionMessage}", ex.Message);
                result.IsValid = false;
            }
            return result;
        }

        private static async Task<HttpStatusCode> PostValidateTokenRequestToPing(
            Dictionary<string, string> request,
            ValidateResponse result,
            PingEndpointSettings endpoint)
        {
            var response = await PostMessage(endpoint, request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    var pingResponse = JsonConvert.DeserializeObject<PingResponse>(await response.Content.ReadAsStringAsync());
                    switch (pingResponse.Active)
                    {
                        case "true":
                            UpdateValidateResponse(result, pingResponse, endpoint.ClientId);
                            break;
                        default:
                            result.IsValid = false;
                            break;
                    }

                    break;
                }
            }
            return response.StatusCode;
        }

        private static void UpdateValidateResponse(ValidateResponse result, PingResponse pingResponse, string clientId)
        {
            var validFrom = DateTime.Now;
            var validTo = validFrom;

            if (int.TryParse(pingResponse.ExpiresIn, out var seconds))
            {
                validTo = DateTime.Now.AddSeconds(seconds);
            }

            result.SecurityToken = new PingSecurityToken(
                validFrom,
                validTo,
                pingResponse.Scope,
                pingResponse.Username ?? clientId);

            result.IsValid = true;
        }

        private static async Task<HttpResponseMessage> PostMessage(
            PingEndpointSettings endpoint, IDictionary<string, string> body)
        {
            var client = HttpClientService.Create(endpoint.Authority);
            using var message = new HttpRequestMessage(HttpMethod.Post, OAuth2Endpoints.OAuthTokenIntrospectionEndpoint)
            {
                Content = new FormUrlEncodedContent(body)
            };
            message.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{endpoint.ClientId}:{endpoint.ClientSecret}")));
            return await client.SendAsync(message);
        }
    }
}
