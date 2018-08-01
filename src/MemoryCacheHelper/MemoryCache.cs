using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System.Collections.Concurrent;
using System.Runtime.Caching;

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
        /// flag indicating whether a setting method is currently being executed
        /// </summary>
        private bool _isSetting = false;

        /// <summary>
        /// The core method that directly sets a value in the wrapped memory cache
        /// </summary>
        /// <param name="key">key of cache item to set</param>
        /// <param name="value">value to set</param>
        /// <param name="policy">optoinal eviction policy</param>
        void IMemoryCacheDirect.Set(string key, object value, CacheItemPolicy policy)
        {
            // TODO: if wiping then lock here

            this._isSetting = true;

            if (policy != null)
            {
                this._memoryCache.Set(key, value, policy);
            }
            else
            {
                this._memoryCache[key] = value;
            }

            this._isSetting = false;
        }

        /// <summary>
        /// The core method that direcly removes an item from the wrapped memory cache
        /// </summary>
        /// <param name="key">key of cache item to remove</param>
        void IMemoryCacheDirect.Remove(string key)
        {
            this._memoryCache.Remove(key);
        }
    }
}
