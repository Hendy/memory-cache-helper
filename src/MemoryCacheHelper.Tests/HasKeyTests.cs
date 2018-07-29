using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class HasKeyTests : BaseTests
    {
        [TestMethod]
        public void Set_Null_Expect_HasKey_False()
        {
            MemoryCache.Instance.Set(KEY, null);

            Assert.IsFalse(MemoryCache.Instance.HasKey(KEY));
        }

        [TestMethod]
        public void Set_Something_Expect_HasKey_True()
        {
            var value = true;

            MemoryCache.Instance.Set(KEY, value);

            Assert.IsTrue(MemoryCache.Instance.HasKey(KEY));
        }
    }
}
