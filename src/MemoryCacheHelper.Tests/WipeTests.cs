using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class WipeTests
    {
        private const string KEY = "exampleCacheKey";

        [TestMethod]
        public void Empty_Wipe()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Populated_Wipe()
        {
            MemoryCache.Instance.Set(KEY, true);

            Assert.IsFalse(MemoryCache.Instance.IsEmpty());

            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }
    }
}
