using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Identity.Security.Abstract;
using Identity.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Identity.Security
{
    /// <summary>
    /// Handles JWT data and other relevant Headers in the current HTTP context
    /// </summary>
    /// <seealso cref="IJwtTokenHelper" />
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private const string _DET = "det";
        private const string _HEADER_JWT = "JwtToken";
        private const string _HEADER_PING_REGION = "TngPingRegion";
        private const string _TOKEN_ISSUER = "OneStreamIntegrations";
        private const string _JWTDEMOTOKEN = "C4CC0DE5-139E-4557-91F0-FAA7DEC2F7DB";

        #region IJwtTokenHelper Members

        /// <summary>Gets the User Context from JWT.</summary>
        /// <param name="jwtToken">The token.</param>
        /// <returns></returns>
        public JwtUserSetting GetUserJwtSetting(string jwtToken)
        {
            var jsonToken = TryReadTokenString(jwtToken);

            var claims = jsonToken?.Claims;
            var claim = claims?
                .FirstOrDefault(x => x.Type == _DET);

            return string.IsNullOrEmpty(claim?.Value)
                ? null 
                : JsonConvert.DeserializeObject<JwtUserSetting>(claim.Value);
        }

        /// <summary>Gets the token from header or cookie.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public string GetJwtTokenFromHeaderOrCookie(HttpRequest request)
        {
            switch (request)
            {
                case null:
                    return null;
            }

            var token = request.Cookies[_HEADER_JWT];
            
            if (string.IsNullOrEmpty(token))
            {
                token = request.Headers[_HEADER_JWT].ToString();
            }

            return token;
        }


        /// <summary>Gets the PING region.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public string GetPingRegion(HttpRequest request) => request?.Headers[_HEADER_PING_REGION];

        /// <summary>Validates JWT token.</summary>
        /// <param name="token">The token.</param>
        /// <param name="secret">The encryption secret.</param>
        /// <returns>True if token is valid</returns>
        public bool ValidateJwtToken(string token, string secret)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _TOKEN_ISSUER,
                ValidateAudience = false,
                ValidAudience = "",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                // For Assessment only checking if the token value is "xyz"
                if (token == _JWTDEMOTOKEN)
                {
                    return true;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return validatedToken != null;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        private static JwtSecurityToken TryReadTokenString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();

            try
            {
                return handler.ReadToken(str) as JwtSecurityToken;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
