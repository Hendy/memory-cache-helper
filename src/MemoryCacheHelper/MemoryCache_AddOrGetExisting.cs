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
        /// Gets or inserts a cache entry into the cache by using a key and a function and optional eviction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="valueFunction">A function to execute to get the value for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        /// <returns></returns>
        public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null)
        {
            bool found;
            T value = this.Get<T>(key, out found);

            if (!found)
            {
                this._cacheKeysBeingHandled.TryAdd(key, new CacheKeyBeingHandled());

                lock (this._cacheKeysBeingHandled[key].Lock)
                {
                    // re-check to see if another thread beat us to setting this value
                    value = this.Get<T>(key, out found);
                    if (!found)
                    {
                        this._cacheKeysBeingHandled[key].Type = typeof(T);

                        // put the function into it's own thread (so it can be cancelled)
                        this._cacheKeysBeingHandled[key].Thread = new Thread(() => {

                            var aborted = false;
                            var success = false;

                            try
                            {
                                value = valueFunction();
                                success = true;
                            }
                            catch (ThreadAbortException)
                            {
                                aborted = true;
                            }
                            finally
                            {
                                if (aborted)
                                {
                                    value = (T)this._cacheKeysBeingHandled[key].Value; // cast should always be valid as this method is the only thing that sets it
                                }
                                else if (success)
                                {
                                    if (value == null)
                                    {
                                        if (!this._isWiping)
                                        {
                                            ((IMemoryCacheDirect)this).Remove(key);
                                        }
                                    }
                                    else
                                    {
                                        ((IMemoryCacheDirect)this).Set(key, value, policy);
                                    }
                                }
                            }
                        });

                        this._cacheKeysBeingHandled[key].Thread.Start();
                        this._cacheKeysBeingHandled[key].Thread.Join();
                    }
                }

                this._cacheKeysBeingHandled.TryRemove(key, out CacheKeyBeingHandled cacheKeyBeingHandled);
            }

            return value;
        }
    }
}
