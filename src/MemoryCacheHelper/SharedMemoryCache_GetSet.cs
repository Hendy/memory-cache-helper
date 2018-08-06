using MemoryCacheHelper.Interfaces;
using MemoryCacheHelper.Models;
using System;
using System.Runtime.Caching;
using System.Threading;

namespace MemoryCacheHelper
{
    public sealed partial class SharedMemoryCache
    {
        /// <summary>
        /// Attempts to get cache value of type T, otherwise sets a cache item by key, function and optional eviction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="valueFunction">A function to execute to get the value for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache entry</param>
        /// <returns>An object of type T, or default T</returns>
        public T GetSet<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null)
        {
            bool found;
            T value = this.Get<T>(key, out found);

            if (!found)
            {
                this._cacheKeysBeingHandled.TryAdd(key, new CacheKeyBeingHandled());

                lock (this._cacheKeysBeingHandled[key].GetSetOperation.Lock)
                {
                    // re-check to see if another thread beat us to setting this value
                    value = this.Get<T>(key, out found);
                    if (!found)
                    {
                        // set, so function can be cancelled if a direct set of the same type occurs
                        this._cacheKeysBeingHandled[key].GetSetOperation.Type = typeof(T);

                        // put the function into it's own thread (so it can be cancelled)
                        this._cacheKeysBeingHandled[key].GetSetOperation.Thread = new Thread(() =>
                        {
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
                                    value = (T)this._cacheKeysBeingHandled[key].GetSetOperation.Value; // cast should always be valid as this method is the only thing that sets it
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

                        this._cacheKeysBeingHandled[key].GetSetOperation.Thread.Start();
                        this._cacheKeysBeingHandled[key].GetSetOperation.Thread.Join();
                    }
                }

                this._cacheKeysBeingHandled.TryRemove(key, out CacheKeyBeingHandled cacheKeyBeingHandled);
            }

            return value;
        }

        /// <summary>
        /// Attempts to get cache value of type T, otherwise sets a cache item by key, value and optional eviction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert</param>
        /// <param name="value">The data for the cache entry</param>
        /// <param name="policy">(Optional) An object that contains eviction details for the cache item</param>
        /// <returns>An object of type T, or default T</returns>
        public T GetSet<T>(string key, T value, CacheItemPolicy policy = null)
        {
            bool found;
            var cachedValue = this.Get<T>(key, out found);

            if (!found)
            {
                this.Set(key, value, policy);

                return value;
            }

            return cachedValue;
        }
    }
}
