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
            ExtendedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Set_Null_Expect_HasKey_False()
        {
            ExtendedMemoryCache.Instance.Set("key", null);

            Assert.IsFalse(ExtendedMemoryCache.Instance.HasKey("key"));
        }

        [TestMethod]
        public void Set_Something_Expect_HasKey_True()
        {
            var value = true;

            ExtendedMemoryCache.Instance.Set("key", value);

            Assert.IsTrue(ExtendedMemoryCache.Instance.HasKey("key"));
        }
    }
}
