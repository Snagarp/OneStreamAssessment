namespace Identity.Security.TokenValidators
{
    /// <summary>
    /// The set of PingFederate URL.
    /// </summary>
    public static class OAuth2Endpoints
    {
        /// <summary>
        /// OAuth Token Endpoint
        /// The name of the OAuth Token Endpoint supporing following action:
        /// 1. Fetching bearer token:
        ///   - Authorization Code Grant Type
        ///   - Refresh Token Grant Type
        ///   - Resource Owner Credentials (Password) Grant Type
        ///   - Client Credentials Grant Type
        ///   - SAML 2.0 Bearer Assertion Grant Type
        /// 2. Validating bearer token:
        ///  - Access Token Verification/Validation Grant Type
        /// </summary>
        public const string OAuthTokenEndpoint = "/as/token.oauth2";

        /// <summary>
        /// OAuth Token Revocation Endpoint
        /// </summary>
        public const string OAuthTokenRevocationEndpoint = "/as/revoke_token.oauth2";

        /// <summary>
        /// OAuth Authorization Endpoint
        /// </summary>
        public const string OAuthAuthorizationEndpoint = "/as/authorization.oauth2";

        /// <summary>
        /// OAuth Grants Endpoint
        /// </summary>
        public const string OAuthGrantsEndpoint = "/as/oauth_access_grants.ping";

        /// <summary>
        /// OAuth Token Introspection Endpoint
        /// </summary>
        public const string OAuthTokenIntrospectionEndpoint = "/as/introspect.oauth2";

        /// <summary>
        /// OAuth Dynamic Client Registration Endpoint
        /// </summary>
        public const string OAuthDynamicClientRegistrationEndpoint = "/as/clients.oauth2";

        /// <summary>
        /// OAuth Dynamic Client Registration Endpoint
        /// </summary>
        public const string OpenIDConnectMetadataEndpoint = "/.well-known/openid-configuration";
    }
}
