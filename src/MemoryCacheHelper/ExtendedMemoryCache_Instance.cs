using MemoryCacheHelper.Models;
using System;
using System.Collections.Concurrent;

namespace MemoryCacheHelper
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class ExtendedMemoryCache
    {
        /// <summary>
        /// Private constructor
        /// </summary>
        private ExtendedMemoryCache()
        {
            this._memoryCache = new System.Runtime.Caching.MemoryCache(Guid.NewGuid().ToString());

            this._cacheKeysBeingHandled = new ConcurrentDictionary<string, CacheKeyBeingHandled>();
        }

        /// <summary>
        /// Get the instance of this cache
        /// </summary>
        public static ExtendedMemoryCache Instance => _lazy.Value;
    }
}
