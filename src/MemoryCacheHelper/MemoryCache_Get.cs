namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Queries key in cache for object of type T
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get</param>
        /// <returns>an object from cache of type T, else default(T)</returns>
        public T Get<T>(string cacheKey)
        {
            bool found;

            return this.Get<T>(cacheKey, out found);
        }

        /// <summary>
        /// Queries key in cache for object of type T
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get</param>
        /// <param name="found">output parameter, indicates whether the return value was found in the cache</param>
        /// <returns>an object from cache of type T, else default(T)</returns>
        public T Get<T>(string cacheKey, out bool found)
        {
            object obj = this._memoryCache[cacheKey];

            if (obj is T)
            {
                found = true;
                return (T)obj;
            }

            found = false;
            return default(T);
        }
    }
}
