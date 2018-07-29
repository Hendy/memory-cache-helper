using System;
using System.Threading;
using System.Threading.Tasks;

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

                lock (((CacheKeyBeingHandled)this._cacheKeysBeingHandled[cacheKey]).Lock)
                {
                    // re-check to see if another thread beat us to setting this value
                    cachedObject = this.Get<T>(cacheKey, out found);
                    if (!found)
                    {
                        ((CacheKeyBeingHandled)this._cacheKeysBeingHandled[cacheKey]).ExpensiveFunctionThread = new Thread(() => {
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
                                    // this doesn't go via this.Remove method, else it'd lock itself
                                    this._memoryCache.Remove(cacheKey);
                                }
                                else
                                {
                                    this._memoryCache[cacheKey] = cachedObject;
                                }
                            }
                        });

                        ((CacheKeyBeingHandled)this._cacheKeysBeingHandled[cacheKey]).ExpensiveFunctionThread.Start();
                        ((CacheKeyBeingHandled)this._cacheKeysBeingHandled[cacheKey]).ExpensiveFunctionThread.Join();
                    }
                }

                object obj;
                this._cacheKeysBeingHandled.TryRemove(cacheKey, out obj);
            }

            return cachedObject;

        }
    }
}
