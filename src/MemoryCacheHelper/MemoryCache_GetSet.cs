using System;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Gets or inserts a cache entry into the cache by using a key and a function and optional eviction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="valueFunction">A function to execute to get the value for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        /// <returns></returns>
        public T GetSet<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null)
        {
            return this.AddOrGetExisting(key, valueFunction, policy);
        }
    }
}
