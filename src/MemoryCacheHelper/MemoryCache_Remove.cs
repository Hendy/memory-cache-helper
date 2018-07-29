using System;
using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Remove an item from cache
        /// </summary>
        /// <param name="cacheKey">the key to the item to remove</param>
        public void Remove(string cacheKey)
        {
            if (this._cacheKeysBeingHandled.TryGetValue(cacheKey, out CacheKeyBeingHandled cacheKeyBeingHandled))
            {
                cacheKeyBeingHandled.ExpensiveFunctionThread.Suspend();

                this._memoryCache.Remove(cacheKey);

                cacheKeyBeingHandled.ExpensiveFunctionThread.Resume();
                cacheKeyBeingHandled.ExpensiveFunctionThread.Abort();
            }
            else
            {
                this._memoryCache.Remove(cacheKey);
            }         
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
                    this.Remove(key);
                }
            }
        }
    }
}
