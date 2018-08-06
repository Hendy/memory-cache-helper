using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class SharedMemoryCache
    {
        /// <summary>
        /// Check to see if the memory cache is empty of keys
        /// </summary>
        /// <returns>Returns true if there are no keys</returns>
        public bool IsEmpty()
        {
            return !this._memoryCache.Any();
        }

        /// <summary>
        /// Check to see if a key exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returns true if the key does not exist</returns>
        internal bool IsEmpty(string key)
        {
            return !this.HasKey(key);
        }
    }
}
