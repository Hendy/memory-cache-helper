namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    { 
        /// <summary>
        /// Immediately sets a cache key, aborting any long running funcation on it
        /// </summary>
        /// <param name="cacheKey">key to cahe item to set</param>
        /// <param name="objectToCache">the object to cache</param>
        public void Set(string cacheKey, object objectToCache)
        {
            if (objectToCache != null)
            {
                if (this._cacheKeysBeingHandled.TryGetValue(cacheKey, out CacheKeyBeingHandled cacheKeyBeingHandled))
                {
                    this._memoryCache[cacheKey] = objectToCache;

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
