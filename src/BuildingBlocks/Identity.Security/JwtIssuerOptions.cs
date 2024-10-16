using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Security
{
    /// <summary>
    /// Represents the options for configuring JWT token issuance, including claims and signing credentials.
    /// </summary>
    public class JwtIssuerOptions
    {
        /// <summary>
        /// Gets or sets the "iss" (Issuer) claim. 
        /// Identifies the principal that issued the JWT.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the "sub" (Subject) claim.
        /// Identifies the principal that is the subject of the JWT.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the "aud" (Audience) claim.
        /// Identifies the recipients that the JWT is intended for.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets the "exp" (Expiration Time) claim.
        /// Identifies the time after which the JWT must not be accepted for processing.
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(ValidFor);

        /// <summary>
        /// Gets the "nbf" (Not Before) claim.
        /// Identifies the time before which the JWT must not be accepted for processing.
        /// </summary>
        public static DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        /// Gets the "iat" (Issued At) claim.
        /// Identifies the time at which the JWT was issued.
        /// </summary>
        public static DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timespan the token will be valid for.
        /// Default value is 24 hours.
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromHours(24);

        /// <summary>
        /// Gets the "jti" (JWT ID) claim generator.
        /// Default value is a GUID generated asynchronously.
        /// </summary>
        public static Func<Task<string>> JtiGenerator =>
            () => Task.FromResult(Guid.NewGuid().ToString());

        /// <summary>
        /// Gets or sets the signing credentials used to sign the JWT.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}
