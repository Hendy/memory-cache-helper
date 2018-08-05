using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System;
using System.Runtime.Caching;
using System.Threading;

namespace MemoryCacheHelper
{
    public sealed partial class ExtendedMemoryCache
    {
        /// <summary>
        /// Set a cache item by key, function and optional eviction
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="valueFunction">A function to execute to get the data for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        public void Set(string key, Func<object> valueFunction, CacheItemPolicy policy = null)
        {
            if (key == null || valueFunction == null) { return; }

            this._cacheKeysBeingHandled.TryAdd(key, new CacheKeyBeingHandled());

            this._cacheKeysBeingHandled[key].SetThreadCounter++;

            lock (this._cacheKeysBeingHandled[key].SetLock)
            {
                if (this._cacheKeysBeingHandled[key].SetThread != null)
                {
                    try
                    {
                        this._cacheKeysBeingHandled[key].SetThread.Abort();
                    }
                    finally
                    {
                        this._cacheKeysBeingHandled[key].SetThread = null;
                    }
                }
            }

            lock (_cacheKeysBeingHandled[key].SetLock)
            {
                if (this._cacheKeysBeingHandled[key].SetThreadCounter > 1)
                {
                    this._cacheKeysBeingHandled[key].SetThreadCounter--;
                }
            }

            if (this._cacheKeysBeingHandled[key].SetThreadCounter > 1)
            {
                return;
            }

            lock (_cacheKeysBeingHandled[key].SetLock)
            { 
                if (this._cacheKeysBeingHandled[key].SetThread == null)
                {
                    this._cacheKeysBeingHandled[key].SetThread = new Thread(() =>
                    {
                        try
                        {
                            object value = valueFunction();

                            ((IMemoryCacheDirect)this).Set(key, value, policy);
                        }
                        catch (ThreadAbortException)
                        {
                        }
                        finally
                        {
                        }
                    });

                    this._cacheKeysBeingHandled[key].SetThread.Start();
                    this._cacheKeysBeingHandled[key].SetThread.Join();
                }
            }
        }

        /// <summary>
        /// Set a cache item by key, value and optional eviction
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="value">The data for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache item</param>
        public void Set(string key, object value, CacheItemPolicy policy = null)
        {
            if (key == null) { return; }

            if (value != null)
            {
                ((IMemoryCacheDirect)this).Set(key, value, policy);

                // if there's currently a function opterating on this key
                if (this._cacheKeysBeingHandled.TryGetValue(key, out CacheKeyBeingHandled cacheKeyBeingHandled))
                {
                    cacheKeyBeingHandled.SetThread?.Abort();

                    // if it wants the same type, then cancel it and give it this value
                    if (cacheKeyBeingHandled.GetSetType == value.GetType())
                    {
                        cacheKeyBeingHandled.Value = value;
                        cacheKeyBeingHandled.GetSetThread.Abort();
                    }
                }
            }
            else
            {
                if (this._isWiping) { return; }

                this.Remove(key);
            }
        }

        /// <summary>
        /// The core method that directly sets a value in the wrapped memory cache
        /// </summary>
        /// <param name="key">key of cache item to set</param>
        /// <param name="value">The data for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        void IMemoryCacheDirect.Set(string key, object value, CacheItemPolicy policy)
        {
            var set = new Action(() => {

                this._isSetting = true;

                policy = policy ?? this.DefaultPolicy;

                try
                {
                    if (policy != null)
                    {
                        this._memoryCache.Set(key, value, policy);
                    }
                    else
                    {
                        this._memoryCache[key] = value;
                    }
                }
                finally
                {
                    this._isSetting = false;
                }

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
