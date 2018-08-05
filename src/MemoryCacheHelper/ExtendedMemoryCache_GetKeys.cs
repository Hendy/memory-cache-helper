using System.Collections.Generic;
using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class ExtendedMemoryCache
    {
        /// <summary>
        /// Iterate though all keys 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetKeys()
        {
            return this._memoryCache.Select(x => x.Key);
        }
    }
}
