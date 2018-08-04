using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Check to see if the memory cache is empty
        /// </summary>
        /// <returns>Returns true if there are no cache keys</returns>
        public bool IsEmpty()
        {
            return !this._memoryCache.Any();
        }
    }
}
