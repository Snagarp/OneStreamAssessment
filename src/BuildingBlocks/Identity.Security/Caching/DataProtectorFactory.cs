using Microsoft.AspNetCore.DataProtection;
using System;

namespace Identity.Security.Caching
{
    /// <summary>
    /// A factory class for creating data protectors used to encrypt and decrypt cached tokens.
    /// </summary>
    public class DataProtectorFactory : IDataProtectorFactory
    {
        /// <summary>
        /// The purpose string used to create a data protector, ensuring that only matching protectors can decrypt the data.
        /// </summary>
        private static readonly string PURPOSE = "CACHE_TOKENS";

        private readonly IDataProtectionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProtectorFactory"/> class.
        /// </summary>
        /// <param name="provider">The data protection provider used to create data protectors.</param>
        public DataProtectorFactory(IDataProtectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Creates a data protector with the specified purpose.
        /// </summary>
        /// <returns>An <see cref="IDataProtector"/> instance configured with the cache token purpose.</returns>
        public IDataProtector CreateProtector() => _provider.CreateProtector(PURPOSE);
    }
}