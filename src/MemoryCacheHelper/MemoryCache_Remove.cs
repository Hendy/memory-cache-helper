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
            // TODO: pause any expensive function on this key
            this._memoryCache.Remove(cacheKey);
            // TODO: cancel any expensive function on this key (it'll return the new value set here)
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
