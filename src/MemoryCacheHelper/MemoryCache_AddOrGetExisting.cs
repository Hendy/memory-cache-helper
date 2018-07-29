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

            if (!found)
            {
                //TODO: prevent a write if currenlty being wiped

                this._cacheKeysBeingHandled.TryAdd(key, new CacheKeyBeingHandled());

                lock (this._cacheKeysBeingHandled[key].Lock)
                {
                    // re-check to see if another thread beat us to setting this value
                    value = this.Get<T>(key, out found);
                    if (!found)
                    {
                        // put the function into it's own thread (so it can be cancelled)
                        this._cacheKeysBeingHandled[key].ValueFunctionThread = new Thread(() => {

                            var aborted = false;

                            try
                            {
                                value = valueFunction();
                            }
                            catch (ThreadAbortException)
                            {
                                aborted = true;
                            }
                            finally
                            {
                                if (aborted)
                                {                                    
                                    value = this.Get<T>(key);
                                }
                                else
                                {
                                    if (value == null)
                                    {
                                        // doesn't go via this.Remove method, else it'd abort itself !
                                        this._memoryCache.Remove(key);
                                    }
                                    else
                                    {
                                        // TODO: adjust policy based on how long the valueFunction took to execute ?

                                        // doesn't go via this.Set method, else it'd abort itself !
                                        if (policy != null)
                                        {
                                            this._memoryCache.Set(key, value, policy);
                                        }
                                        else
                                        {
                                            this._memoryCache[key] = value;
                                        }
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
