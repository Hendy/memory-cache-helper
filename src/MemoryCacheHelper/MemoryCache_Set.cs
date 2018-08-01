using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System;
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
                ((IMemoryCacheDirect)this).Set(key, value, policy);

                // if there's currently a function opterating on this key
                if (this._cacheKeysBeingHandled.TryGetValue(key, out CacheKeyBeingHandled cacheKeyBeingHandled))
                {
                    // if it wants the same type, then cancel it and give it this value
                    if (cacheKeyBeingHandled.Type == value.GetType())
                    {
                        cacheKeyBeingHandled.Value = value;
                        cacheKeyBeingHandled.Thread.Abort();
                    }
                }
            }
            else
            {
                if (this._isWiping) { return; }

                this.Remove(key);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="valueFunction"></param>
        ///// <param name="policy"></param>
        //public void Set(string key, Func<object> valueFunction, CacheItemPolicy policy = null)
        //{

        //}
    }
}
