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
            SharedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Boolean()
        {
            bool input = true;

            SharedMemoryCache.Instance.Set("key", input);
            Assert.AreEqual(true, SharedMemoryCache.Instance.Get<bool>("key"));
        }

        [TestMethod]
        public void Integer()
        {
            int input = 1;

            SharedMemoryCache.Instance.Set("key", input);
            Assert.AreEqual(1, SharedMemoryCache.Instance.Get<int>("key"));
        }

        [TestMethod]
        public void Enum()
        {
            DayOfWeek input = DayOfWeek.Friday;

            SharedMemoryCache.Instance.Set("key", input);
            Assert.AreEqual(DayOfWeek.Friday, SharedMemoryCache.Instance.Get<DayOfWeek>("key"));
        }

    }
}
