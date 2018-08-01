using System.Collections.Generic;
using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Gets all keys
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetKeys()
        {
            return this._memoryCache.Select(x => x.Key);
        }
    }
}
