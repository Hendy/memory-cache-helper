using MemoryCacheHelper.Interfaces;
using System;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache : IMemoryCacheDirect
    {
        /// <summary>
        /// If not found, inserts a cache entry into the cache by using a key and a function and optional eviction
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="valueFunction">A function to execute to get the value for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        public void Add(string key, Func<object> valueFunction, CacheItemPolicy policy = null)
        {
            // direct key check here, as the Set check for existance expects the value to be of given type
            if (!this.HasKey(key))
            {
                this.Set(key, valueFunction, policy);
            }
        }

        /// <summary>
        /// If not found, inserts a cache entry into the cache by using a key and a value and optional eviction
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="value">A function to execute to get the value for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        public void Add(string key, object value, CacheItemPolicy policy = null)
        {
            if (!this.HasKey(key))
            {
                this.Set(key, value, policy);
            }
        }
    }
}
