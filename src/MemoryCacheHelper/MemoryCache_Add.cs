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

        //public void Add(string key, object value, CacheItemPolicy policy = null)
        //{
        //    if (!this.HasKey(key))
        //    {
        //        this.Set(key, value, policy);
        //    }
        //}
    }
}
