using MemoryCacheHelper.Interfaces;
using System;
using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// The core method that direcly removes an item from the wrapped memory cache
        /// </summary>
        /// <param name="key">key of cache item to remove</param>
        void IMemoryCacheDirect.Remove(string key)
        {
            this._memoryCache.Remove(key);
        }

        /// <summary>
        /// Remove an item from cache
        /// </summary>
        /// <param name="key">the key to the item to remove</param>
        public void Remove(string key)
        {
            if (this._isWiping) { return; }

            ((IMemoryCacheDirect)this).Remove(key);
        }

        /// <summary>
        /// Remove all items from cache for which the lambda returns true
        /// </summary>
        /// <param name="keyEvaluationFunction">function to test a string cache key, and return true if it should be removed from cache</param>
        public void Remove(Func<string, bool> keyEvaluationFunction)
        {
            if (this._isWiping) { return; }

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
