using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class TypeTests : BaseTests
    {
        [TestMethod]
        public void Boolean()
        {
            bool input = true;

            MemoryCache.Instance.Set(KEY, input);
            Assert.AreEqual(true, MemoryCache.Instance.Get<bool>(KEY));
        }

        [TestMethod]
        public void Integer()
        {
            int input = 1;

            MemoryCache.Instance.Set(KEY, input);
            Assert.AreEqual(1, MemoryCache.Instance.Get<int>(KEY));
        }

        [TestMethod]
        public void Enum()
        {
            DayOfWeek input = DayOfWeek.Friday;

            MemoryCache.Instance.Set(KEY, input);
            Assert.AreEqual(DayOfWeek.Friday, MemoryCache.Instance.Get<DayOfWeek>(KEY));
        }

    }
}
