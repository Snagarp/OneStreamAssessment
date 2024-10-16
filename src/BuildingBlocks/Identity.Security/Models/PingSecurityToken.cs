using System;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Security.Models
{
    /// <summary>
    /// Represents a security token for use with PingFederate, containing token validity details and user-specific metadata.
    /// </summary>
    public class PingSecurityToken : SecurityToken
    {
        private readonly string _id;
        private readonly DateTime _validFrom;
        private readonly DateTime _validTo;
        private readonly string _scope;
        private readonly string _username;
        private const string _issuer = "PingFederate";

        /// <summary>
        /// Initializes a new instance of the <see cref="PingSecurityToken"/> class with the specified parameters.
        /// </summary>
        /// <param name="validFrom">The date and time from which the token is valid.</param>
        /// <param name="validTo">The date and time until which the token is valid.</param>
        /// <param name="scope">The scope associated with the token.</param>
        /// <param name="username">The username associated with the token.</param>
        public PingSecurityToken(DateTime validFrom, DateTime validTo, string scope, string username)
        {
            _id = Guid.NewGuid().ToString();
            _validFrom = validFrom;
            _validTo = validTo;
            _scope = scope;
            _username = username;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingSecurityToken"/> class with the specified ID and parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the token.</param>
        /// <param name="validFrom">The date and time from which the token is valid.</param>
        /// <param name="validTo">The date and time until which the token is valid.</param>
        /// <param name="scope">The scope associated with the token.</param>
        /// <param name="username">The username associated with the token.</param>
        internal PingSecurityToken(string id, DateTime validFrom, DateTime validTo, string scope, string username)
        {
            _id = id;
            _validFrom = validFrom;
            _validTo = validTo;
            _scope = scope;
            _username = username;
        }

        /// <summary>
        /// Gets the unique identifier for the token.
        /// </summary>
        public override string Id => _id;

        /// <summary>
        /// Gets the issuer of the token, which is PingFederate.
        /// </summary>
        public override string Issuer => _issuer;

        /// <summary>
        /// Gets the <see cref="SecurityKey"/> associated with the token.
        /// This property is not implemented in this class.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown when accessed.</exception>
        public override SecurityKey SecurityKey => throw new NotImplementedException();

        /// <summary>
        /// Gets or sets the signing key used to sign the token.
        /// This property is not implemented in this class.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown when accessed or set.</exception>
        public override SecurityKey SigningKey
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the date and time from which the token is valid.
        /// </summary>
        public override DateTime ValidFrom => _validFrom;

        /// <summary>
        /// Gets the date and time until which the token is valid.
        /// </summary>
        public override DateTime ValidTo => _validTo;

        /// <summary>
        /// Gets the scope associated with the token.
        /// </summary>
        public string Scope => _scope;

        /// <summary>
        /// Gets the username associated with the token.
        /// </summary>
        public string Username => _username;
    }
}
