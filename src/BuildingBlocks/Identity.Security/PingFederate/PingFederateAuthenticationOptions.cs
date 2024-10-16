using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Identity.Security.PingFederate
{
    /// <summary>
    ///     Represents the default implementation of configuration for Ping Federate Authentication module.
    /// </summary>
    public class PingFederateAuthenticationOptions
    {
        private const string _DEFAULT_ENDPOINT_KEY = "US";

        private IDictionary<string, PingEndpointSettings> _endpoints;

        /// <summary>Gets or sets the default connection.</summary>
        /// <value>The default connection.</value>
        public PingEndpointSettings DefaultEndpoint
        {
            get => Endpoints[_DEFAULT_ENDPOINT_KEY];
            set => Endpoints[_DEFAULT_ENDPOINT_KEY] = value;
        }

        /// <summary>
        ///     It true, cache will be used to cache the token.
        /// </summary>
        public bool UseDistributedCache { get; set; }

        /// <summary>
        ///     It true, it will encrypt the token before it is preserved in cache.
        /// </summary>
        public bool UseDataProtection { get; set; }

        /// <summary>
        ///     Time to expiry value in cache.
        /// </summary>
        public int CachedTokenExpirationTimeInMinutes { get; set; }

        /// <summary>Gets Ping endpoints.</summary>
        /// <value>The authorities.</value>
        public IDictionary<string, PingEndpointSettings> Endpoints =>
            _endpoints ??= new ConcurrentDictionary<string, PingEndpointSettings>();

        /// <summary>
        /// Gets or sets a value indicating whether to use default authority.
        /// </summary>
        /// <value><c>true</c> to use default endpoint; otherwise, <c>false</c>.</value>
        public bool UseDefaultEndpoint { get; set; }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat(CultureInfo.InvariantCulture, "Configuration of : {0}", nameof(PingFederateAuthenticationOptions));
            sb.AppendLine();
            sb.AppendFormat(CultureInfo.InvariantCulture, "UseDataProtection: {0}", UseDataProtection);
            sb.AppendLine();
            sb.AppendFormat(CultureInfo.InvariantCulture, "UseDistributedCache: {0}", UseDistributedCache);
            sb.AppendLine();
            sb.AppendFormat(CultureInfo.InvariantCulture, "CachedTokenExpirationTimeInMinutes: {0}", CachedTokenExpirationTimeInMinutes);

            foreach (var pair in Endpoints)
            {
                sb.AppendLine();
                sb.AppendFormat(CultureInfo.InvariantCulture, "Authority: {0} - {1}", pair.Key, pair.Value.Authority);
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Defines Ping Endpoint Settings
    /// </summary>
    public sealed class PingEndpointSettings
    {
        /// <summary>
        ///     The uri of the Authorization Server
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        ///     The client identifier issued to the client during the registration.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     The client secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
