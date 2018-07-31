using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    { 
        /// <summary>
        /// Immediately set a cache item (aborting any long running function that may be operating on it)
        /// </summary>
        /// <param name="key">key to cahe item to set</param>
        /// <param name="value">the object to cache</param>
        public void Set(string key, object value, CacheItemPolicy policy = null)
        {
            if (this._isWiping) { return; }

            if (value != null)
            {
                ((IMemoryCacheDirect)this).Set(key, value, policy);

                if (this._cacheKeysBeingHandled.TryGetValue(key, out CacheKeyBeingHandled cacheKeyBeingHandled))
                {
                    cacheKeyBeingHandled.AbortedValue = value;
                    cacheKeyBeingHandled.ValueFunctionThread.Abort();
                }
            }
            else
            {
                this.Remove(key);
            }
        }
    }
}
