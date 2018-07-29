using System.Runtime.Caching;

namespace MemoryCacheHelper.Interfaces
{
    /// <summary>
    /// interface used so caller can target a specific method (which would normally conflict with an existing override)
    /// </summary>
    internal interface IMemoryCacheDirect
    {
        /// <summary>
        /// This set method sets the wrapped memory cache directly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="policy"></param>
        void Set(string key, object value, CacheItemPolicy policy);

        /// <summary>
        /// This method removes from the wrapped memory cache directly
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}
