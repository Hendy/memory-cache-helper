using System;
using System.Runtime.Caching;
using System.Threading;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// This method is to replace the GetSet method as the word Set implies that it's result should always be written to the cache,
        /// however the GetSet locks incase another call hasn't yet finished, in which case it's result wouldn't be written to the cache, 
        /// so it behaves more the Add word (as used on System.Runtime.Caching.MemoryCache)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueFunction"></param>
        /// <param name="policy">optional CacheItemPolicy</param>
        /// <returns></returns>
        public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null)
        {
            bool found;
            T cachedObject = this.Get<T>(key, out found);

            if (!found)
            {
                //TODO: prevent a write if currenlty being wiped

                this._cacheKeysBeingHandled.TryAdd(key, new CacheKeyBeingHandled()); // TODO: handle unexpected fails to add

                lock (this._cacheKeysBeingHandled[key].Lock)
                {
                    // re-check to see if another thread beat us to setting this value
                    cachedObject = this.Get<T>(key, out found);
                    if (!found)
                    {
                        // put the expensive function into it's own thread (so it can be cancelled)
                        this._cacheKeysBeingHandled[key].ExpensiveFunctionThread = new Thread(() => {

                            var aborted = false;

                            try
                            {
                                cachedObject = valueFunction();
                            }
                            catch (ThreadAbortException)
                            {
                                aborted = true;
                            }
                            finally
                            {
                                if (aborted)
                                {                                    
                                    cachedObject = this.Get<T>(key);
                                }
                                else
                                {
                                    if (cachedObject == null)
                                    {
                                        // doesn't go via this.Remove method, else it'd abort itself !
                                        this._memoryCache.Remove(key);
                                    }
                                    else
                                    {
                                        // doesn't go via this.Set method, else it'd abort itself !
                                        if (policy != null)
                                        {
                                            this._memoryCache.Set(key, cachedObject, policy);
                                        }
                                        else
                                        {
                                            this._memoryCache[key] = cachedObject;
                                        }
                                    }
                                }
                            }
                        });

                        this._cacheKeysBeingHandled[key].ExpensiveFunctionThread.Start();
                        this._cacheKeysBeingHandled[key].ExpensiveFunctionThread.Join();
                    }
                }

                this._cacheKeysBeingHandled.TryRemove(key, out CacheKeyBeingHandled cacheKeyBeingHandled); // TODO: handle unepxected fails to remove
            }

            return cachedObject;
        }
    }
}
