using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Inserts a cache entry into the cache by using a key and a value and eviction
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="value">The data for the cache entry</param>
        /// <param name="policy">(Optionl) An object that contains eviction details for the cache entry. This object provides more options for eviction than a simple absolute expiration</param>
        public void Set(string key, object value, CacheItemPolicy policy = null)
        {
            if (value != null)
            {
                // TODO: lock but only if currently wiping

                ((IMemoryCacheDirect)this).Set(key, value, policy);

                if (this._cacheKeysBeingHandled.TryGetValue(key, out CacheKeyBeingHandled cacheKeyBeingHandled))
                {
                    // if this set was faster than any currently executing function (attempting to set the same type) then
                    // cancel it, and let it return this value too


                    cacheKeyBeingHandled.ValueFunctionThread.Abort();
                }
            }
            else
            {
                if (this._isWiping) { return; }

                this.Remove(key);
            }
        }
    }
}
