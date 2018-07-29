using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// locker for the wipe method (no need to have them running concurrently)
        /// Whislt wiping, nothing is written to the cache, and all responses return null
        /// </summary>
        private object _wipeLock = new object();

        /// <summary>
        /// Wipe all cache items
        /// </summary>
        internal void Wipe()
        {
            lock (this._wipeLock)
            {
                this._isWiping = true;

                if (!this.IsEmpty())
                {
                    foreach (var key in this._memoryCache.Select(x => x.Key))
                    {
                        this.Remove(key);
                    }
                }

                this._isWiping = false;
            }
        }
    }
}
