namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Queries key in cache for object of type T
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="key">key to the cache item to get</param>
        /// <returns>an object from cache of type T, else default(T)</returns>
        public T Get<T>(string key)
        {
            return this.Get<T>(key, out bool found);
        }

        /// <summary>
        /// Queries key in cache for object of type T
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="key">key to the cache item to get</param>
        /// <param name="found">output parameter, indicates whether the return value was found in the cache and of the expected type</param>
        /// <returns>an object from cache of type T, else default(T)</returns>
        public T Get<T>(string key, out bool found)
        {
            object value = this._memoryCache[key];

            if (value is T)
            {
                found = true;
                return (T)value;
            }

            found = false;
            return default(T);
        }
    }
}
