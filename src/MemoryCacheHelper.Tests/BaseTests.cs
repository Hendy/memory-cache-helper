using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public abstract class BaseTests
    {
        protected const string KEY = "exampleCacheKey";

        /// <summary>
        /// Every test should start with an empty cache
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        /// <summary>
        /// Add a number of cache items, using random guid key, and datetime value
        /// </summary>
        /// <param name="count">the number of items to set</param>
        protected void SetSomeItems(int count)
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
