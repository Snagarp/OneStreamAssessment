using System.Threading.Tasks;

namespace Identity.Security.Caching
{
    /// <summary>
    ///     Creating the interface for CustomCacheProvider with methods to Set and Get the cache
    ///     This interface will be used betweent the TechData services and Distributed cache
    ///     In future, if the distributed cache need to be changed, no change required in the service
    ///     Only change need to be performed in the existing implementation of this interface / create an ew implementation and
    ///     register for DI
    /// </summary>
    public interface ICustomCacheProvider
    {
        /// <summary>
        ///     Set the value against the cache key
        /// </summary>
        /// <typeparam name="T">Type of the object that need to saved in cache</typeparam>
        /// <param name="key">Key name in which the cache should be retained</param>
        /// <param name="value">Object that need to be saved in cache</param>
        /// <param name="durationCategory">Duration in Timespan for which the cache need to be retained</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        void Set<T>(string key, T value, string durationCategory, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Set the value against the cache key
        /// </summary>
        /// <typeparam name="T">Type of the object that need to saved in cache</typeparam>
        /// <param name="key">Key name in which the cache should be retained</param>
        /// <param name="value">Object that need to be saved in cache</param>
        /// <param name="durationCategory">Duration in Timespan for which the cache need to be retained</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        void DistributedSet<T>(string key, T value, string durationCategory, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Asynchronously set the value against the cache key
        /// </summary>
        /// <typeparam name="T">Type of the object that need to saved in cache</typeparam>
        /// <param name="key">Key name in which the cache should be retained</param>
        /// <param name="value">Object that need to be saved in cache</param>
        /// <param name="durationCategory">Duration in Timespan for which the cache need to be retained</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value, string durationCategory, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Set the value against the cache key without duration
        /// </summary>
        /// <typeparam name="T">Type of the object that need to saved in cache</typeparam>
        /// <param name="key">Key name in which the cache should be retained</param>
        /// <param name="value">Object that need to be saved in cache</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        void Set<T>(string key, T value, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Asynchronously set the value against the cache key without duration
        /// </summary>
        /// <typeparam name="T">Type of the object that need to saved in cache</typeparam>
        /// <param name="key">Key name in which the cache should be retained</param>
        /// <param name="value">Object that need to be saved in cache</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Get the cached object based on the key
        /// </summary>
        /// <typeparam name="T">Type of the object that need to be cached</typeparam>
        /// <param name="key">Key name in which the cache should be retained</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        /// <returns></returns>
        T Get<T>(string key, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Get the cached object based on the key
        /// </summary>
        /// <typeparam name="T">Type of the object that need to be cached</typeparam>
        /// <param name="key">Key name in which the cache should be retained</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        /// <returns></returns>
        T DistributedGet<T>(string key, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Asynchronously get the cached object based on the key
        /// </summary>
        /// <typeparam name="T">Type of the object that need to be cached</typeparam>
        /// <param name="key">Key name in which the cache should be retained</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Remove the cached object based on the key
        /// </summary>
        /// <param name="key">Key name in which the cache object is retained</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        void Remove(string key, CacheKeyAccess cacheKeyAccess);

        /// <summary>
        ///     Asynchronously remove the cached object based on the key
        /// </summary>
        /// <param name="key">Key name in which the cache object is retained</param>
        /// <param name="cacheKeyAccess">To identify how the cache key need to be logically kept</param>
        /// <returns></returns>
        Task RemoveAsync(string key, CacheKeyAccess cacheKeyAccess);
    }
}
