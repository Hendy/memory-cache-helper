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
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueFunction"></param>
        /// <param name="policy">optional CacheItemPolicy</param>
        /// <returns></returns>
        public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null)
        {
            bool found;
            T value = this.Get<T>(key, out found);

            if (!found && !this._isWiping)
            {
                this._cacheKeysBeingHandled.TryAdd(key, new CacheKeyBeingHandled());

                lock (this._cacheKeysBeingHandled[key].Lock)
                {
                    // re-check to see if another thread beat us to setting this value
                    value = this.Get<T>(key, out found);
                    if (!found && !this._isWiping)
                    {
                        // put the function into it's own thread (so it can be cancelled)
                        this._cacheKeysBeingHandled[key].ValueFunctionThread = new Thread(() => {

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
                                    var abortedValue = this._cacheKeysBeingHandled[key].AbortedValue;

                                    if (abortedValue is T)
                                    {
                                        value = (T)abortedValue;
                                    }
                                    else
                                    {
                                        value = default(T);
                                    }
                                }
                                else if (success)
                                {
                                    if (value == null)
                                    {
                                        ((IMemoryCacheDirect)this).Remove(key);
                                    }
                                    else
                                    {
                                        ((IMemoryCacheDirect)this).Set(key, value, policy);
                                    }
                                }
                            }
                        });

                        this._cacheKeysBeingHandled[key].ValueFunctionThread.Start();
                        this._cacheKeysBeingHandled[key].ValueFunctionThread.Join();
                    }
                }

                this._cacheKeysBeingHandled.TryRemove(key, out CacheKeyBeingHandled cacheKeyBeingHandled);
            }

            return value;
        }
    }
}
