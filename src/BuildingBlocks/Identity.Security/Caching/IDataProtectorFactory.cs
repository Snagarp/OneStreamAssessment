using Microsoft.AspNetCore.DataProtection;

namespace Identity.Security.Caching
{
    /// <summary>
    /// Defines a contract for creating <see cref="IDataProtector"/> instances 
    /// used for data encryption and decryption.
    /// </summary>
    public interface IDataProtectorFactory
    {
        /// <summary>
        /// Creates a new <see cref="IDataProtector"/> instance.
        /// </summary>
        /// <returns>An <see cref="IDataProtector"/> that can be used to encrypt and decrypt data.</returns>
        IDataProtector CreateProtector();
    }
}