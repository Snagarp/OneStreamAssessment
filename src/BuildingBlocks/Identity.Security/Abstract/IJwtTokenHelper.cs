using Identity.Security.Models;
using Microsoft.AspNetCore.Http;

namespace Identity.Security.Abstract
{
    /// <summary>
    /// Provides helper methods for working with JWT tokens, including extraction, validation, and settings retrieval.
    /// </summary>
    public interface IJwtTokenHelper
    {
        /// <summary>
        /// Retrieves the JWT user settings based on the provided token.
        /// </summary>
        /// <param name="jwtToken">The JWT token used to extract user settings.</param>
        /// <returns>A <see cref="JwtUserSetting"/> object containing the user's JWT settings.</returns>
        JwtUserSetting GetUserJwtSetting(string jwtToken);

        /// <summary>
        /// Extracts the JWT token from either the HTTP header or cookies of the given request.
        /// </summary>
        /// <param name="request">The HTTP request containing the token in its headers or cookies.</param>
        /// <returns>The JWT token as a string if found; otherwise, an empty string.</returns>
        string GetJwtTokenFromHeaderOrCookie(HttpRequest request);

        /// <summary>
        /// Retrieves the Ping region information from the given HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request containing the Ping region information.</param>
        /// <returns>A string representing the Ping region, or an empty string if not found.</returns>
        string GetPingRegion(HttpRequest request);

        /// <summary>
        /// Validates the given JWT token using the provided secret.
        /// </summary>
        /// <param name="token">The JWT token to be validated.</param>
        /// <param name="secret">The secret key used to validate the token.</param>
        /// <returns>
        /// <c>true</c> if the token is valid and not expired; otherwise, <c>false</c>.
        /// </returns>
        bool ValidateJwtToken(string token, string secret);
    }
}