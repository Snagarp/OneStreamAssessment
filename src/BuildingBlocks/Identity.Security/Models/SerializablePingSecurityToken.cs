using System;

namespace Identity.Security.Models
{
    /// <summary>
    /// A serializable version of the <see cref="PingSecurityToken"/> class, used for persisting or transmitting token data.
    /// </summary>
    [Serializable]
    public class SerializablePingSecurityToken
    {
        /// <summary>
        /// Gets or sets the unique identifier for the token.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time from which the token is valid.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which the token is valid.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Gets or sets the scope associated with the token.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the username associated with the token.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializablePingSecurityToken"/> class.
        /// </summary>
        public SerializablePingSecurityToken() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializablePingSecurityToken"/> class using an existing <see cref="PingSecurityToken"/>.
        /// </summary>
        /// <param name="token">The <see cref="PingSecurityToken"/> instance to copy data from.</param>
        public SerializablePingSecurityToken(PingSecurityToken token)
        {
            switch (token)
            {
                case null:
                    throw new ArgumentNullException(nameof(token));
            }

            Id = token.Id;
            ValidFrom = token.ValidFrom;
            ValidTo = token.ValidTo;
            Scope = token.Scope;
            Username = token.Username;
        }

        /// <summary>
        /// Converts this instance of <see cref="SerializablePingSecurityToken"/> back to a <see cref="PingSecurityToken"/>.
        /// </summary>
        /// <returns>A new <see cref="PingSecurityToken"/> instance with the same data.</returns>
        public PingSecurityToken ToPingSecurityToken() => new(Id, ValidFrom, ValidTo, Scope, Username);
    }
}
