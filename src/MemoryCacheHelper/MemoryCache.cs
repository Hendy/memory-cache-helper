using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache : IMemoryCacheDirect
    {
        /// <summary>
        /// Set the default cache item policy to use if one isn't provided in the call
        /// </summary>
        public CacheItemPolicy DefaultCacheItemPolicy { get; set; } = null;

        /// <summary>
        /// Internal instance of the <see cref="System.Runtime.Caching.MemoryCache"/> class
        /// </summary>
        private System.Runtime.Caching.MemoryCache _memoryCache;

        /// <summary>
        /// Locker collection of all cache keys currently executing a function to set a cache item
        /// </summary>
        private ConcurrentDictionary<string, CacheKeyBeingHandled> _cacheKeysBeingHandled;

        /// <summary>
        /// State flag indicating whether the wipe method is currently being executed
        /// </summary>
        private bool _isWiping = false;

        /// <summary>
        /// State flag indicating whether any non-waitable set action is currently being executed
        /// </summary>
        private bool _isSetting = false;
    }
}
