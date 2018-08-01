using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System.Collections.Concurrent;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache : IMemoryCacheDirect
    {
        /// <summary>
        /// Locker collection of all cache keys currently executing a function to set a cache item
        /// </summary>
        private ConcurrentDictionary<string, CacheKeyBeingHandled> _cacheKeysBeingHandled;

        /// <summary>
        /// Internal instance of the <see cref="System.Runtime.Caching.MemoryCache"/> class
        /// </summary>
        private System.Runtime.Caching.MemoryCache _memoryCache;

        /// <summary>
        /// flag indicating whether the wipe method is currenly being executed
        /// </summary>
        private bool _isWiping = false;

        /// <summary>
        /// flag indicating whether non waitable set action is occuring
        /// </summary>
        private bool _isSetting = false;
    }
}
