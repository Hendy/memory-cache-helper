using MemoryCacheHelper.Interfaces;
using System;
using System.Runtime.Caching;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache : IMemoryCacheDirect
    {   
        public void Add<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null)
        {
            this.AddOrGetExisting(key, valueFunction, policy);
        }
    }
}
