using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System;
using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Remove an item from cache
        /// </summary>
        /// <param name="key">the key to the item to remove</param>
        public void Remove(string key)
        {
            ((IMemoryCacheDirect)this).Remove(key);

            if (this._cacheKeysBeingHandled.TryGetValue(key, out CacheKeyBeingHandled cacheKeyBeingHandled))
            {
                cacheKeyBeingHandled.ValueFunctionThread.Abort();
            }
        }

        /// <summary>
        /// Remove all items from cache for which the lambda returns true
        /// </summary>
        /// <param name="keyEvaluationFunction">function to test a string cache key, and return true if it should be removed from cache</param>
        public void Remove(Func<string, bool> keyEvaluationFunction)
        {
            foreach (string key in this._memoryCache.Select(x => x.Key))
            {
                if (keyEvaluationFunction(key))
                {
                    this.Remove(key);
                }
            }
        }
    }
}
