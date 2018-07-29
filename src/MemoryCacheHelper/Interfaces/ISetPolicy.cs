using System.Runtime.Caching;

namespace MemoryCacheHelper.Interfaces
{
    /// <summary>
    /// interface used so caller can target a specific Set method (which would normally conflict with an existing override)
    /// </summary>
    internal interface ISetPolicy
    {
        /// <summary>
        /// This set method updates the wrapped memory cache directly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="policy"></param>
        void Set(string key, object value, CacheItemPolicy policy);
    }
}
