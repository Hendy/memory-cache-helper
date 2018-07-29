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
                // TODO: pause any expensive function on this key
                this._memoryCache[cacheKey] = objectToCache;
                // TODO: cancel any expensive function on this key (it'll return the new value set here)
            }
            else
            {
                this.Remove(cacheKey);
            }
        }
    }
}
