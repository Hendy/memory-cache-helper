using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    { 
        /// <summary>
        /// Immediately sets a cache key, aborting any long running funcation on it
        /// </summary>
        /// <param name="cacheKey">key to cahe item to set</param>
        /// <param name="objectToCache">the object to cache</param>
        public void Set(string cacheKey, object objectToCache, CacheItemPolicy policy = null)
        {
            // TODO: prevent a write if a wipe is currently taking place

            if (objectToCache != null)
            {
                if (policy != null)
                {
                    this._memoryCache.Set(cacheKey, objectToCache, policy);
                }
                else
                {
                    this._memoryCache[cacheKey] = objectToCache;
                }

                // abort any expensive funcs attempting to set this key
                if (this._cacheKeysBeingHandled.TryGetValue(cacheKey, out CacheKeyBeingHandled cacheKeyBeingHandled))
                {
                    cacheKeyBeingHandled.ExpensiveFunctionThread.Abort();
                }
            }
            else
            {
                this.Remove(cacheKey);
            }
        }
    }
}
