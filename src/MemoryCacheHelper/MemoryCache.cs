using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Locker collection of all cache keys currently having their 'expensive functions' evaluated
        /// string = cacheKey, object = used as a locking object
        /// </summary>
        private ConcurrentDictionary<string, CacheKeyBeingHandled> _cacheKeysBeingHandled;

        /// <summary>
        /// locker for the wipe method (no need to have them running concurrently)
        /// </summary>
        private object _wipeLock = new object();

        /// <summary>
        /// Internal instance of the <see cref="System.Runtime.Caching.MemoryCache"/> class
        /// </summary>
        private System.Runtime.Caching.MemoryCache _memoryCache;

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
        /// Wipe all cache items
        /// </summary>
        internal void Wipe()
        {
            lock(this._wipeLock)
            {
                if (!this.IsEmpty())
                {
                    foreach (var key in this._memoryCache.Select(x => x.Key))
                    {
                        this.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// See if there are any cache items
        /// </summary>
        /// <returns>true if the local cache is empty</returns>
        internal bool IsEmpty()
        {
            return !this._memoryCache.Any();
        }
    }
}
