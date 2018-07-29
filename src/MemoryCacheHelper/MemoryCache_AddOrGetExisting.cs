using System;
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
        /// <param name="cacheKey"></param>
        /// <param name="expensiveFunction"></param>
        /// <returns></returns>
        public T AddOrGetExisting<T>(string cacheKey, Func<T> expensiveFunction)
        {
            bool found;
            T cachedObject = this.Get<T>(cacheKey, out found);

            if (!found)
            {
                this._cacheKeysBeingHandled.TryAdd(cacheKey, new CacheKeyBeingHandled()); // TODO: handle unexpected fails to add

                lock (this._cacheKeysBeingHandled[cacheKey].Lock)
                {
                    // re-check to see if another thread beat us to setting this value
                    cachedObject = this.Get<T>(cacheKey, out found);
                    if (!found)
                    {
                        this._cacheKeysBeingHandled[cacheKey].ExpensiveFunctionThread = new Thread(() => {
                            try
                            {
                                cachedObject = expensiveFunction();
                            }
                            catch (ThreadAbortException)
                            {
                                // the thread was aborted becuase something else changed the cache key this long running function was trying to set
                                cachedObject = this.Get<T>(cacheKey);
                            }
                            finally
                            {
                                if (cachedObject == null)
                                {
                                    // doesn't go via this.Remove method, else it'd suspend itself
                                    this._memoryCache.Remove(cacheKey);
                                }
                                else
                                {
                                    // doesn't go via this.Set method, else it'd suspend itself
                                    this._memoryCache[cacheKey] = cachedObject;
                                }
                            }
                        });

                        this._cacheKeysBeingHandled[cacheKey].ExpensiveFunctionThread.Start();
                        this._cacheKeysBeingHandled[cacheKey].ExpensiveFunctionThread.Join();
                    }
                }

                this._cacheKeysBeingHandled.TryRemove(cacheKey, out CacheKeyBeingHandled cacheKeyBeingHandled); // TODO: handle unepxected fails to remove
            }

            return cachedObject;
        }
    }
}
