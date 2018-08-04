using MemoryCacheHelper.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Flush will remove all cache entries, but not block any set operations
        /// </summary>
        public void Flush()
        {
            if (this._isWiping) { return; }

            var keys = this._memoryCache.Select(x => x.Key);
            
            Parallel.ForEach(keys, x => ((IMemoryCacheDirect)this).Remove(x));
        }
    }
}
