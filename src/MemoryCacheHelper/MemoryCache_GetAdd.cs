using System;

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
        /// <param name="expensiveFunc"></param>
        /// <returns></returns>
        public T GetAdd<T>(string cacheKey, Func<T> expensiveFunc)
        {
            bool found;
            T cachedObject = this.Get<T>(cacheKey, out found);

            if (!found)
            {
                this._cacheKeysBeingHandled.TryAdd(cacheKey, new object()); // object used as a locker

                lock (this._cacheKeysBeingHandled[cacheKey])
                {
                    // re-check to see if another thread beat us to setting this value
                    cachedObject = this.Get<T>(cacheKey, out found);
                    if (!found)
                    {
                        // TODO: this expensiveFunc() needs to be cancellable, and for this to then return whatever was put into cache by something else
                        cachedObject = expensiveFunc();

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
                }

                object obj;
                this._cacheKeysBeingHandled.TryRemove(cacheKey, out obj);
            }

            return cachedObject;

        }
    }
}
