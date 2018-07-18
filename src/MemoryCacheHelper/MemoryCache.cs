using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed class MemoryCache
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
        /// Locker collection of all cache keys currently having their 'expensive functions' evaluated
        /// string = cacheKey, object = used as a locking object
        /// </summary>
        private ConcurrentDictionary<string, object> _cacheKeysBeingHandled;

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
        /// Helper for unit tests to expose the memory cache name
        /// </summary>
        internal string Name => this._memoryCache.Name;

        /// <summary>
        /// Queries key in cache for object of type T
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get</param>
        /// <returns>an object from cache of type T, else default(T)</returns>
        public T Get<T>(string cacheKey)
        {
            bool found;

            return this.Get<T>(cacheKey, out found);
        }

        /// <summary>
        /// Queries key in cache for object of type T
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get</param>
        /// <param name="found">output parameter, indicates whether the return value was found in the cache</param>
        /// <returns>an object from cache of type T, else default(T)</returns>
        public T Get<T>(string cacheKey, out bool found)
        {
            object obj = this._memoryCache[cacheKey];

            if (obj is T)
            {
                found = true;
                return (T)obj;
            }

            found = false;
            return default(T);
        }

        /// <summary>
        /// Queries key in cache for object of type T, otherwise executes expensiveFunc and ensures cache is set
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get (or set if not found)</param>
        /// <param name="expensiveFunc">function to execute if cache item not found</param>
        /// <param name="timeout">(optional) number of seconds before cache value should time out, default 0 = no timeout</param>
        /// <returns>an object from cache of type T, else the result of the expensiveFunc</returns>
        public T GetSet<T>(string cacheKey, Func<T> expensiveFunc, int timeout = 0)
        {
            bool found;
            T cachedObject = this.Get<T>(cacheKey, out found);

            if (!found)
            {
                this._cacheKeysBeingHandled.TryAdd(cacheKey, new object()); // object used as a locker

                lock (this._cacheKeysBeingHandled[cacheKey])
                {
                    // re-check to see if another thread beat us to setting this value
                    cachedObject = this.Get<T>(cacheKey, out found);
                    if (!found)
                    {
                        cachedObject = expensiveFunc();

                        this.Set(cacheKey, cachedObject, timeout);
                    }
                }

                object obj;
                this._cacheKeysBeingHandled.TryRemove(cacheKey, out obj);
            }

            return cachedObject;
        }

        /// <summary>
        /// Populate the cache key with the supplied object
        /// </summary>
        /// <param name="cacheKey">key for the cache item to set</param>
        /// <param name="objectToCache">object to put into the cache item</param>
        /// <param name="timeout">(optional) number of seconds before cache value should time out, default 0 = no timeout</param>
        public void Set(string cacheKey, object objectToCache, int timeout = 0)
        {
            if (objectToCache != null)
            {
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
        }

        /// <summary>
        /// Remove an item from cache
        /// </summary>
        /// <param name="cacheKey">the key to the item to remove</param>
        public void Remove(string cacheKey)
        {
            this._memoryCache.Remove(cacheKey);
        }

        /// <summary>
        /// Remove all items from cache for which the lambda returns true
        /// </summary>
        /// <param name="lambda">function to test a string cache key, and return true if it should be removed from cache</param>
        public void Remove(Func<string, bool> lambda)
        {
            foreach (string key in this._memoryCache.Select(x => x.Key))
            {
                if (lambda(key))
                {
                    this._memoryCache.Remove(key);
                }
            }
        }

        /// <summary>
        /// Wipe all cache items
        /// </summary>
        internal void Wipe()
        {
            foreach (var key in this._memoryCache.Select(x => x.Key))
            {
                this._memoryCache.Remove(key);
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
