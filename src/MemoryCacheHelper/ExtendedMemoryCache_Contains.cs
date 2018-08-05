namespace MemoryCacheHelper
{
    public sealed partial class ExtendedMemoryCache
    {
        /// <summary>
        /// Determines whether a cache entry exists in the cache
        /// </summary>
        /// <param name="key">the cache key to look for</param>
        /// <returns>true if the supplied cache key was found</returns>
        public bool Contains(string key)
        {
            return this._memoryCache.Contains(key);
        }
    }
}
