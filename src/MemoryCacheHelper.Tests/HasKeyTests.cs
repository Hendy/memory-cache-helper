using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class HasKeyTests
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
        public void Set_Null_Expect_HasKey_False()
        {
            SharedMemoryCache.Instance.Set("key", null);

            Assert.IsFalse(SharedMemoryCache.Instance.HasKey("key"));
        }

        [TestMethod]
        public void Set_Something_Expect_HasKey_True()
        {
            var value = true;

            SharedMemoryCache.Instance.Set("key", value);

            Assert.IsTrue(SharedMemoryCache.Instance.HasKey("key"));
        }
    }
}
