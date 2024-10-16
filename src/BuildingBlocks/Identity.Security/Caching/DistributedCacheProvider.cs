using System;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Identity.Security.Caching
{
    /// <summary>
    /// Provides functionality to interact with a distributed cache using data protection for encryption and decryption of cached data.
    /// </summary>
    public interface IDistributedCacheProvider
    {
        /// <summary>
        /// Stores the specified data in the cache with an optional expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the data to be cached.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="dataToCache">The data to be stored in the cache.</param>
        /// <param name="absoluteExpiration">Optional expiration time. If not provided, a default expiration time will be used.</param>
        void Set<T>(string key, T dataToCache, TimeSpan? absoluteExpiration = null);

        /// <summary>
        /// Retrieves the specified data from the cache.
        /// </summary>
        /// <typeparam name="T">The type of the data to be retrieved.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>The cached data if found; otherwise, the default value of type <typeparamref name="T"/>.</returns>
        T Get<T>(string key);
    }

    /// <summary>
    /// A concrete implementation of <see cref="IDistributedCacheProvider"/> that interacts with a distributed cache using data protection.
    /// </summary>
    public class DistributedCacheProvider : IDistributedCacheProvider
    {
        private static readonly string KEY_PREFIX = "OAuthToken";
        private readonly ILogger<DistributedCacheProvider> _logger;
        private readonly IDistributedCache _cache;
        private readonly IDataProtector _protector;
        private readonly TimeSpan _defaultExpiration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedCacheProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used to log operations and errors.</param>
        /// <param name="cache">The distributed cache instance.</param>
        /// <param name="dataProtectorFactory">The data protector factory used to create data protectors.</param>
        /// <param name="options">The options monitor to retrieve cache expiration settings.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the required parameters are null.</exception>
        public DistributedCacheProvider(
            ILogger<DistributedCacheProvider> logger,
            IDistributedCache cache,
            IDataProtectorFactory dataProtectorFactory,
            IOptionsMonitor<DataProtectorOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            dataProtectorFactory = dataProtectorFactory ?? throw new ArgumentNullException(nameof(dataProtectorFactory));

            _protector = dataProtectorFactory.CreateProtector();
            _defaultExpiration = TimeSpan.FromMinutes(options.CurrentValue.CachedTokenExpirationTimeInMinutes);
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            try
            {
                var data = _cache.Get($"{KEY_PREFIX}:{key}");
                if (data != null)
                {
                    _logger.LogInformation($"Cache hit. Loaded {data.Length} bytes from cache.");

                    if (_protector != null)
                        data = _protector.Unprotect(data);

                    var json = FromByteArray(data);
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading from the cache.");
                TryRemove(key); // Attempt to remove the key if an error occurs.
            }
            return default;
        }

        /// <inheritdoc />
        public void Set<T>(string key, T dataToCache, TimeSpan? absoluteExpiration = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dataToCache);
                var data = ToByteArray(json);

                if (_protector != null)
                    data = _protector.Protect(data);

                _cache.Set(
                    $"{KEY_PREFIX}:{key}",
                    data,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = absoluteExpiration ?? _defaultExpiration
                    });

                _logger.LogInformation($"Cached {data.Length} bytes successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while writing to the cache.");
            }
        }

        /// <summary>
        /// Attempts to remove the specified key from the cache.
        /// </summary>
        /// <param name="key">The cache key to remove.</param>
        private void TryRemove(string key)
        {
            try
            {
                _cache.Remove($"{KEY_PREFIX}:{key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to remove a cache entry.");
            }
        }

        /// <summary>
        /// Converts a JSON string to a byte array.
        /// </summary>
        /// <param name="json">The JSON string to convert.</param>
        /// <returns>A byte array representation of the JSON string.</returns>
        private static byte[] ToByteArray(string json)
        {
            switch (json)
            {
                case null:
                    return null;
                default:
                    try
                    {
                        using var ms = new MemoryStream();
                        using var writer = new StreamWriter(ms);
                        writer.Write(json);
                        writer.Flush();
                        return ms.ToArray();
                    }
                    catch
                    {
                        return null;
                    }
            }
        }

        /// <summary>
        /// Converts a byte array to a JSON string.
        /// </summary>
        /// <param name="data">The byte array to convert.</param>
        /// <returns>A JSON string representation of the byte array.</returns>
        private static string FromByteArray(byte[] data)
        {
            switch (data)
            {
                case null:
                    return string.Empty;
                default:
                    try
                    {
                        using var ms = new MemoryStream(data);
                        using var reader = new StreamReader(ms);
                        return reader.ReadToEnd();
                    }
                    catch
                    {
                        return string.Empty;
                    }
            }
        }
    }
}
