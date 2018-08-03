﻿using MemoryCacheHelper.Models;
using System;
using System.Collections.Concurrent;

namespace MemoryCacheHelper
{
    /// <summary>
    /// Responsible for constructing the instance
    /// </summary>
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Private constructor
        /// </summary>
        private MemoryCache()
        {
            this._memoryCache = new System.Runtime.Caching.MemoryCache(Guid.NewGuid().ToString());

            this._cacheKeysBeingHandled = new ConcurrentDictionary<string, CacheKeyBeingHandled>();
        }

        /// <summary>
        /// Get the instance of this cache
        /// </summary>
        public static MemoryCache Instance => _lazy.Value;
    }
}
