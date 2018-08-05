using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class TypeTests
    {
        /// <summary>
        /// Every test should start with an empty cache
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            MemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Boolean()
        {
            bool input = true;

            MemoryCache.Instance.Set("key", input);
            Assert.AreEqual(true, MemoryCache.Instance.Get<bool>("key"));
        }

        [TestMethod]
        public void Integer()
        {
            int input = 1;

            MemoryCache.Instance.Set("key", input);
            Assert.AreEqual(1, MemoryCache.Instance.Get<int>("key"));
        }

        [TestMethod]
        public void Enum()
        {
            DayOfWeek input = DayOfWeek.Friday;

            MemoryCache.Instance.Set("key", input);
            Assert.AreEqual(DayOfWeek.Friday, MemoryCache.Instance.Get<DayOfWeek>("key"));
        }

    }
}
