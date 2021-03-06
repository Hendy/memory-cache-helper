﻿using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class SharedMemoryCache : IMemoryCacheDirect
    {
        /// <summary>
        /// Singleton instance of the <see cref="SharedMemoryCache"/> class
        /// </summary>
        private static readonly Lazy<SharedMemoryCache> _lazy = new Lazy<SharedMemoryCache>(() => new SharedMemoryCache());

        /// <summary>
        /// The wrapped memory cache
        /// </summary>
        private MemoryCache _memoryCache;

        /// <summary>
        /// Optional default cache item policy to use if one isn't provided in each call
        /// </summary>
        public CacheItemPolicy DefaultPolicy { get; set; } = null;

        /// <summary>
        /// Collection of all cache keys currently executing a function to set a cache item
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
