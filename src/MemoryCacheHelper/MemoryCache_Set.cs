using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System;
using System.Runtime.Caching;
using System.Threading;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Locker object
        /// </summary>
        private object _setLock = new object();

        /// <summary>
        /// Inserts a cache entry into the cache by using a key and a value and optional eviction
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="value">The data for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
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

        /// <summary>
        /// The core method that directly sets a value in the wrapped memory cache
        /// </summary>
        /// <param name="key">key of cache item to set</param>
        /// <param name="value">value to set</param>
        /// <param name="policy">optoinal eviction policy</param>
        void IMemoryCacheDirect.Set(string key, object value, CacheItemPolicy policy)
        {
            var set = new Action(() => {

                this._isSetting = true;

                policy = policy ?? this.DefaultCacheItemPolicy;

                if (policy != null)
                {
                    this._memoryCache.Set(key, value, policy);
                }
                else
                {
                    this._memoryCache[key] = value;
                }

                this._isSetting = false;

            });

            if (!this._isWiping)
            {
                set();
            }
            else // hold the set until wiping is complete
            {
                lock (this._setLock)
                {
                    SpinWait.SpinUntil(() => !this._isWiping); // one thread watching until wipe complete

                    set();
                }
            }
        }
    }
}
