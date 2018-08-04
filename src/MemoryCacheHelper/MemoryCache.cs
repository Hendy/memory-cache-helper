using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache : IMemoryCacheDirect
    {
        /// <summary>
        /// Singleton instance of the <see cref="MemoryCache"/> class
        /// </summary>
        private static readonly Lazy<MemoryCache> _lazy = new Lazy<MemoryCache>(() => new MemoryCache());

        /// <summary>
        /// Internal instance of the <see cref="System.Runtime.Caching.MemoryCache"/> class
        /// </summary>
        private System.Runtime.Caching.MemoryCache _memoryCache;

        /// <summary>
        /// Optional default cache item policy to use if one isn't provided in each call
        /// </summary>
        public CacheItemPolicy DefaultCacheItemPolicy { get; set; } = null;

        /// <summary>
        /// Locker collection of all cache keys currently executing a function to set a cache item
        /// </summary>
        private ConcurrentDictionary<string, CacheKeyBeingHandled> _cacheKeysBeingHandled;

        /// <summary>
        /// Locker object
        /// </summary>
        private object _wipeLock = new object();

        /// <summary>
        /// State flag indicating whether the wipe method is currently being executed
        /// </summary>
        private bool _isWiping = false;

        /// <summary>
        /// Locker object, used to lock set operations when a wipe is in progress
        /// </summary>
        private object _setLock = new object();

        /// <summary>
        /// State flag indicating whether any non-waitable set action is currently being executed
        /// </summary>
        private bool _isSetting = false;
    }
}
