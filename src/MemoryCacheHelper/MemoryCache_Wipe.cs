using MemoryCacheHelper.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Wipe will remove all items from cache, blocking any sets until the wipe is complete
        /// </summary>
        public void Wipe()
        {
            if (this._isWiping) { return; }

            lock (this._wipeLock)
            {
                if (this._isWiping) { return; }

                this._isWiping = true;

                SpinWait.SpinUntil(() => !this._isSetting); // wait until all running set operations are complete (new ones are blocked)

                var keys = this._memoryCache.Select(x => x.Key);

                Parallel.ForEach(keys, x => ((IMemoryCacheDirect)this).Remove(x));

                try
                {
                    if (keys.Any())
                    {
                        throw new Exception("Unable to remove keys: " + string.Join(", ", keys));
                    }
                }
                finally
                {
                    this._isWiping = false;
                }
            }
        }
    }
}
