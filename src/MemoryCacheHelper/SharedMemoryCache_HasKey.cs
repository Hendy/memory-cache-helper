namespace MemoryCacheHelper
{
    public sealed partial class SharedMemoryCache
    {
        /// <summary>
        /// See if the cache contains a specific cache key
        /// </summary>
        /// <param name="key">the cache key to look for</param>
        /// <returns>true if the supplied cache key was found</returns>
        public bool HasKey(string key)
        {
            return this._memoryCache[key] != null;
        }
    }
}
