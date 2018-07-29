using System.Linq;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// locker for the wipe method (no need to have them running concurrently)
        /// </summary>
        private object _wipeLock = new object();

        /// <summary>
        /// Wipe all cache items
        /// </summary>
        internal void Wipe()
        {
            lock (this._wipeLock)
            {
                if (!this.IsEmpty())
                {
                    foreach (var key in this._memoryCache.Select(x => x.Key))
                    {
                        this.Remove(key);
                    }
                }
            }
        }
    }
}
