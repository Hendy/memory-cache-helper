namespace MemoryCacheHelper
{
    public sealed partial class ExtendedMemoryCache
    {
        /// <summary>
        /// Removes a specified percentage of cache entries from the cache object
        /// </summary>
        /// <param name="percent">The percentage of total cache entries to remove</param>
        /// <returns>The number of entries removed from the cache</returns>
        public long Trim(int percent)
        {
            return this._memoryCache.Trim(percent);
        }
    }
}