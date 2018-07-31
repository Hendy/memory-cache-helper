using MemoryCacheHelper.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// locker for the wipe method (no need to have them running concurrently)
        /// Whislt wiping, nothing else should write to cache
        /// </summary>
        private object _wipeLock = new object();

        /// <summary>
        /// Wipe will remove all items from cache, whilst this processes is in operation nothing else should write to cache
        /// </summary>
        internal void Wipe()
        {
            lock (this._wipeLock)
            {
                this._isWiping = true;

                Parallel.ForEach(this._cacheKeysBeingHandled, x => {
                    x.Value.AbortedValue = null;
                    x.Value.ValueFunctionThread.Abort();
                });

                while (!this.IsEmpty())
                {
                    var keys = this._memoryCache.Select(x => x.Key);

                    Parallel.ForEach(keys, x => ((IMemoryCacheDirect)this).Remove(x));
                }

                this._isWiping = false;
            }
        }
    }
}
