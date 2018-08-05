using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class RemoveTests
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
        public void Remove_Unknown_Key()
        {
            ExtendedMemoryCache.Instance.Remove("key");
        }

        [TestMethod]
        public void Remove_By_Lambda()
        {
            for(int i = 0; i < 10; i ++)
            {
                var uniqueKey = "key" + i.ToString();

                ExtendedMemoryCache.Instance.Set(uniqueKey, true);
            }

            Assert.IsFalse(ExtendedMemoryCache.Instance.IsEmpty());

            ExtendedMemoryCache.Instance.Remove(x => x.StartsWith("key"));

            Assert.IsTrue(ExtendedMemoryCache.Instance.IsEmpty());
        }
    }
}
