namespace Identity.Security.Caching
{
    /// <summary>
    /// Enumerator used to identify the scope of the cache key is
    /// </summary>
	public enum CacheKeyAccess
    {
        /// <summary>
        /// Keep the cache key as private logically to this service by appending a text
        /// </summary>
		Private = 0,

        /// <summary>
        /// Keep the cache key globally accessible by using the key that was provided for cache
        /// </summary>
		Global = 1
    }
}
