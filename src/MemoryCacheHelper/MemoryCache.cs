using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

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
        /// Queries key in cache for object of type T, otherwise executes expensiveFunc and ensures cache is set
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get (or set if not found)</param>
        /// <param name="expensiveFunc">function to execute if cache item not found</param>
        /// <param name="timeout">(optional) number of seconds before cache value should time out, default 0 = no timeout</param>
        /// <returns>an object from cache of type T, else the result of the expensiveFunc</returns>
        [Obsolete("Use AddOrGetExisting<T>(string, Func<T>) instead")]
        public T GetSet<T>(string cacheKey, Func<T> expensiveFunc, int timeout = 0)
        {
            bool found;
            T cachedObject = this.Get<T>(cacheKey, out found);

            if (!found)
            {
                this._cacheKeysBeingHandled.TryAdd(cacheKey, new CacheKeyBeingHandled());

                lock (this._cacheKeysBeingHandled[cacheKey].Lock)
                {
                    // re-check to see if another thread beat us to setting this value
                    cachedObject = this.Get<T>(cacheKey, out found);
                    if (!found)
                    {
                        cachedObject = expensiveFunc();

                        this.Set(cacheKey, cachedObject, timeout);
                    }
                }

                CacheKeyBeingHandled cacheKeyBeingHandled;
                this._cacheKeysBeingHandled.TryRemove(cacheKey, out cacheKeyBeingHandled);
            }

            return cachedObject;
        }

        /// <summary>
        /// Populate the cache key with the supplied object - supplying null will remove cache item
        /// </summary>
        /// <param name="cacheKey">key for the cache item to set</param>
        /// <param name="objectToCache">object to put into the cache item, null will remove cache item</param>
        /// <param name="timeout">number of seconds before cache value should time out, default 0 = no timeout</param>
        [Obsolete("Use Set(string, object) instead")]
        public void Set(string cacheKey, object objectToCache, int timeout)
        {
            if (objectToCache != null)
            {
                // TODO: cancel any existing expensive function that may be setting this key (as we have object ready to cache here)
 
                if (timeout > 0)
                {
                    this._memoryCache.Set(
                        cacheKey,
                        objectToCache,
                        new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(timeout) });
                }
                else
                {
                    this._memoryCache[cacheKey] = objectToCache;
                }
            }
            else
            {
                this.Remove(cacheKey);
            }
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
