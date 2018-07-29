namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="objectToCache"></param>
        public void Set(string cacheKey, object objectToCache)
        {
            if (objectToCache != null)
            {
                if (this._cacheKeysBeingHandled.TryGetValue(cacheKey, out object value))
                {
                    var cacheKeyBeingHandled = (CacheKeyBeingHandled)value;

                    cacheKeyBeingHandled.ExpensiveFunctionThread.Suspend();

                    this._memoryCache[cacheKey] = objectToCache;

                    cacheKeyBeingHandled.ExpensiveFunctionThread.Resume();
                    cacheKeyBeingHandled.ExpensiveFunctionThread.Abort();
                }
                else
                {
                    this._memoryCache[cacheKey] = objectToCache;
                }
            }
            else
            {
                this.Remove(cacheKey);
            }
        }
    }
}
