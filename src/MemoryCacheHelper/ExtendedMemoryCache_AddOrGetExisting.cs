using System;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class ExtendedMemoryCache
    {
        /// <summary>
        /// If key not found, sets a new cache item by key, function and optional eviction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="valueFunction">A function to execute to get the value for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        /// <returns>An object of type T, or default T</returns>
        public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null)
        {
            if (!this.HasKey(key))
            {
                // GetSet calls can be cancelled, where as Set calls expect to always run
                return this.GetSet<T>(key, valueFunction, policy);
            }

            return this.Get<T>(key);
        }

        /// <summary>
        /// If key not found, sets a new cache item by key, value and optional eviction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="value">A value for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        /// <returns>An object of type T, or default T</returns>
        public T AddOrGetExisting<T>(string key, T value, CacheItemPolicy policy = null)
        {
            if (!this.HasKey(key))
            {
                return this.GetSet<T>(key, value, policy);
            }

            return this.Get<T>(key);
        }
    }
}
