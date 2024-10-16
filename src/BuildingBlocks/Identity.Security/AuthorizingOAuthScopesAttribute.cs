using System;
using System.Collections.Generic;

namespace Identity.Security
{
    /// <summary>
    /// Defines OAuth Identity Scopes for Authentication
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizingOAuthScopesAttribute : Attribute
    {
        private readonly string[] _scopes;

        /// <summary>Gets or sets the identity scopes.</summary>
        /// <value>The scopes.</value>
        public IReadOnlyList<string> Scopes => _scopes;

        /// <summary>Gets or sets a value indicating whether to verify ping only.</summary>
        /// <value><c>true</c> to verify ping only; otherwise, <c>false</c>.</value>
        public bool VerifyPingOnly { get; set; }

        /// <summary>Initializes a new instance of the <see cref="AuthorizingOAuthScopesAttribute"/> class.</summary>
        /// <param name="scopes">The authorized scopes.</param>
        public AuthorizingOAuthScopesAttribute(params string[] scopes)
        {
            switch (scopes)
            {
                case null:
                    _scopes = Array.Empty<string>();
                    break;
                default:
                    _scopes = new string[scopes.Length];
                    scopes.CopyTo(_scopes, 0);
                    break;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="AuthorizingOAuthScopesAttribute"/> class.</summary>
        /// <param name="verifyPingOnly">if set to <c>true</c> then verifies Ping authentication only.</param>
        /// <param name="scopes">The identity scopes.</param>
        public AuthorizingOAuthScopesAttribute(bool verifyPingOnly = false, params string[] scopes) : this(scopes)
        {
            VerifyPingOnly = verifyPingOnly;
        }
    }
}
