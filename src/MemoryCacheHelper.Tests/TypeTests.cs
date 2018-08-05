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
            ExtendedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Boolean()
        {
            bool input = true;

            ExtendedMemoryCache.Instance.Set("key", input);
            Assert.AreEqual(true, ExtendedMemoryCache.Instance.Get<bool>("key"));
        }

        [TestMethod]
        public void Integer()
        {
            int input = 1;

            ExtendedMemoryCache.Instance.Set("key", input);
            Assert.AreEqual(1, ExtendedMemoryCache.Instance.Get<int>("key"));
        }

        [TestMethod]
        public void Enum()
        {
            DayOfWeek input = DayOfWeek.Friday;

            ExtendedMemoryCache.Instance.Set("key", input);
            Assert.AreEqual(DayOfWeek.Friday, ExtendedMemoryCache.Instance.Get<DayOfWeek>("key"));
        }

    }
}
