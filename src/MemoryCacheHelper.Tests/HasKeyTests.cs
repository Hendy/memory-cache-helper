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
            MemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Set_Null_Expect_HasKey_False()
        {
            MemoryCache.Instance.Set("key", null);

            Assert.IsFalse(MemoryCache.Instance.HasKey("key"));
        }

        [TestMethod]
        public void Set_Something_Expect_HasKey_True()
        {
            var value = true;

            MemoryCache.Instance.Set("key", value);

            Assert.IsTrue(MemoryCache.Instance.HasKey("key"));
        }
    }
}
