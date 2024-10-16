using Azure.Security.KeyVault.Secrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VendorConfiguration.Application.Services
{
    /// <summary>
    /// Provides utility methods to interact with Azure Key Vault for retrieving secrets.
    /// </summary>
    public class KeyVaultUtility : IKeyVault
    {
        private readonly SecretClient _client;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultUtility"/> class.
        /// </summary>
        /// <param name="client">The <see cref="SecretClient"/> used to interact with Azure Key Vault.</param>
        /// <param name="logger">The logger used for logging information and errors.</param>
        public KeyVaultUtility(SecretClient client, ILogger<KeyVaultUtility> logger)
        {
            _client = ArgumentGuard.NotNull(client, nameof(client));
            _logger = ArgumentGuard.NotNull(logger, nameof(logger));
        }

        /// <summary>
        /// Retrieves a secret from Azure Key Vault as a plain text string.
        /// </summary>
        /// <param name="key">The key (name) of the secret to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the secret as a string, or null if the secret is not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is null or empty.</exception>
        public async Task<string?> GetSecret(string key)
        {
            ArgumentGuard.NotNullOrEmpty(key, nameof(key));

            var secret = await _client.GetSecretAsync(key).ConfigureAwait(false);
            return secret?.Value.Value;
        }

        /// <summary>
        /// Retrieves a secret from Azure Key Vault and parses it as JSON if applicable.
        /// </summary>
        /// <param name="key">The key (name) of the secret to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a dynamic object if the secret is valid JSON, or null if the secret is not found or not in JSON format.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="key"/> is null or empty.</exception>
        /// <exception cref="JsonReaderException">Thrown when the secret content is not a valid JSON.</exception>
        /// <exception cref="Exception">Thrown when there is an error while retrieving the secret.</exception>
        public async Task<dynamic?> GetSecretAsJson(string key)
        {
            ArgumentGuard.NotNullOrEmpty(key, nameof(key));

            _logger.LogInformation("Getting secret {key} as JSON", key);

            try
            {
                var secret = await _client.GetSecretAsync(key).ConfigureAwait(false);
                return secret switch
                {
                    null => null,
                    _ => (secret.Value.Properties.ContentType?.Equals("application/json") ?? false) switch
                    {
                        true => JObject.Parse(secret.Value.Value),
                        _ => null,
                    },
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting secret {key} as JSON", key);
                throw;
            }
        }
    }
}
