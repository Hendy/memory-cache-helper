using System;

namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// This method is to replace the GetSet method as the word Set implies that it's result should always be written to the cache,
        /// however the GetSet locks incase another call hasn't yet finished, in which case it's result wouldn't be written to the cache, 
        /// so it behaves more the Add word (as used on System.Runtime.Caching.MemoryCache)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="expensiveFunc"></param>
        /// <returns></returns>
        public T GetAdd<T>(string cacheKey, Func<T> expensiveFunc)
        {
            throw new NotImplementedException();
        }    
    }
}
