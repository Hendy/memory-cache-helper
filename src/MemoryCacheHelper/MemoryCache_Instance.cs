using System;
using System.Collections.Concurrent;

namespace MemoryCacheHelper
{
    /// <summary>
    /// Responsible for constructing the instance, and specifying class scope vars
    /// </summary>
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Singleton instance of the <see cref="MemoryCache"/> class
        /// </summary>
        private static readonly Lazy<MemoryCache> _lazy = new Lazy<MemoryCache>(() => new MemoryCache());

        /// <summary>
        /// Locker collection of all cache keys currently having their 'expensive functions' evaluated
        /// string = cacheKey, object = used as a locking object
        /// </summary>
        private ConcurrentDictionary<string, object> _cacheKeysBeingHandled;

        /// <summary>
        /// Internal instance of the <see cref="System.Runtime.Caching.MemoryCache"/> class
        /// </summary>
        private System.Runtime.Caching.MemoryCache _memoryCache;

        /// <summary>
        /// Private constructor
        /// </summary>
        private MemoryCache()
        {
            this._memoryCache = new System.Runtime.Caching.MemoryCache(Guid.NewGuid().ToString());

            this._cacheKeysBeingHandled = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// Get the instance of this cache
        /// </summary>
        public static MemoryCache Instance => _lazy.Value;

        /// <summary>
        /// The unique name of this memory cache
        /// </summary>
        public string Name => this._memoryCache.Name;
    }
}
