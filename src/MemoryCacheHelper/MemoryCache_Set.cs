using MemoryCacheHelper.Interfaces;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    { 
        /// <summary>
        /// Immediately sets a cache key, aborting any long running funcation on it
        /// </summary>
        /// <param name="key">key to cahe item to set</param>
        /// <param name="value">the object to cache</param>
        public void Set(string key, object value, CacheItemPolicy policy = null)
        {
            // TODO: prevent a write if a wipe is currently taking place

            if (value != null)
            {
                ((ISetDirect)this).Set(key, value, policy);

                // abort any expensive funcs attempting to set this key
                if (this._cacheKeysBeingHandled.TryGetValue(key, out CacheKeyBeingHandled cacheKeyBeingHandled))
                {
                    cacheKeyBeingHandled.ValueFunctionThread.Abort();
                }
            }
            else
            {
                this.Remove(key);
            }
        }
    }
}
