using MemoryCacheHelper.Interfaces;
using System;
using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Remove items from cache where a supplied function returns true for any given key
        /// </summary>
        /// <param name="keyFunction">function to test a string cache key, and return true if it should be removed from cache</param>
        public void Remove(Func<string, bool> keyFunction)
        {
            if (this._isWiping) { return; }

            foreach (string key in this._memoryCache.Select(x => x.Key))
            {
                if (keyFunction(key))
                {
                    this.Remove(key);
                }
            }
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
        /// The core method that direcly removes an item from the wrapped memory cache
        /// </summary>
        /// <param name="key">key of cache item to remove</param>
        void IMemoryCacheDirect.Remove(string key)
        {
            this._memoryCache.Remove(key);
        }
    }
}
