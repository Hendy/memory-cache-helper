using MemoryCacheHelper.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// locker for the wipe method
        /// Whislt wiping, nothing else should write to cache
        /// </summary>
        private object _wipeLock = new object();

        /// <summary>
        /// Wipe will remove all items from cache, whilst this processes is in operation nothing else should write to cache
        /// </summary>
        internal void Wipe()
        {
            if (this._isWiping) { return; }

            lock (this._wipeLock)
            {
                if (this._isWiping) { return; }

                this._isWiping = true;

                var keys = this._memoryCache.Select(x => x.Key);

                Parallel.ForEach(keys, x => ((IMemoryCacheDirect)this).Remove(x));

                if (keys.Any())
                {
                    throw new Exception();
                }

                this._isWiping = false;
            }
        }
    }
}
