﻿using System;
using System.Collections.Concurrent;

namespace MemoryCacheHelper
{
    /// <summary>
    /// Responsible for constructing the instance
    /// </summary>
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Singleton instance of the <see cref="MemoryCache"/> class
        /// defined in this file, as this is the only place it's used
        /// </summary>
        private static readonly Lazy<MemoryCache> _lazy = new Lazy<MemoryCache>(() => new MemoryCache());

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
