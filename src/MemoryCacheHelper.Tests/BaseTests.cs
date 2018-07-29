using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public abstract class BaseTests
    {
        protected const string KEY = "exampleCacheKey";

        [TestInitialize]
        public void Initialize()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }
    }
}
