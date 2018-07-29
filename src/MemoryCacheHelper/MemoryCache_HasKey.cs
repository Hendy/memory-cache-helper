namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// See if the cache contains a specific cache key
        /// </summary>
        /// <param name="cacheKey">the cache key to look for</param>
        /// <returns>true if the supplied cache key was found</returns>
        public bool HasKey(string cacheKey)
        {
            return this._memoryCache[cacheKey] != null;
        }
    }
}
