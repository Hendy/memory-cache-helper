using System;

namespace MemoryCacheHelper.Tests
{
    internal static class TestHelper
    {
        /// <summary>
        /// Add a number of cache items, using random guid key, and datetime value
        /// </summary>
        /// <param name="count">the number of items to set</param>
        internal static void SetSomeItems(int count)
        {
            count = Math.Max(0, count);

            for (int i = 0; i < count; i++)
            {
                var key = Guid.NewGuid().ToString();
                var value = DateTime.Now;

                MemoryCache.Instance.Set(key, value);
            }
        }
    }
}
