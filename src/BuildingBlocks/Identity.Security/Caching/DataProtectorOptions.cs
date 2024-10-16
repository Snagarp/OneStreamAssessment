namespace Identity.Security.Caching
{
    /// <summary>
    /// Represents configuration options for data protection related to caching tokens.
    /// </summary>
    public class DataProtectorOptions
    {
        /// <summary>
        /// Gets or sets the expiration time, in minutes, for cached tokens.
        /// </summary>
        /// <value>
        /// The time in minutes after which a cached token will expire. 
        /// The default value is 5 minutes.
        /// </value>
        public int CachedTokenExpirationTimeInMinutes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProtectorOptions"/> class 
        /// with default values.
        /// </summary>
        public DataProtectorOptions()
        {
            CachedTokenExpirationTimeInMinutes = 5;
        }
    }
}