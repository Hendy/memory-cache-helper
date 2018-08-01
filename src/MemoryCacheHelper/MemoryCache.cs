using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        /// Enumerates sorting the collection of cache keys
        /// (not in it's own partial yet, as not public)
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetOrderedKeys()
        {
            return this._memoryCache.Select(x => x.Key).OrderBy(x => x);
        }

        /// <summary>
        /// The core method that directly sets a value in the wrapped memory cache
        /// only accessable via internal interface (couldn't be private, as would be an override conflict)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="policy"></param>
        void IMemoryCacheDirect.Set(string key, object value, CacheItemPolicy policy)
        {
            if (this._isWiping) { return; }
            
            if (policy != null)
            {
                this._memoryCache.Set(key, value, policy);
            }
            else
            {
                this._memoryCache[key] = value;
            }
        }

        /// <summary>
        /// The core method that direcly removes an item from the wrapped memory cache
        /// only accessable via internal interface (couldn't be private, as would be an override conflict)
        /// </summary>
        /// <param name="key"></param>
        void IMemoryCacheDirect.Remove(string key)
        {
            this._memoryCache.Remove(key);
        }
    }
}
