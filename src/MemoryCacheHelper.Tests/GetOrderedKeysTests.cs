using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class GetOrderedKeysTests
    {
        private const string KEY = "exampleCacheKey";

        [TestInitialize]
        public void Initialize()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Set_Out_Of_Order_Then_GetOrderedKeys()
        {
            MemoryCache.Instance.Set("key3", true);
            MemoryCache.Instance.Set("key1", true);
            MemoryCache.Instance.Set("key2", true);

            var keys = MemoryCache.Instance.GetOrderedKeys().ToArray();

            Assert.AreEqual(3, keys.Length);

            Assert.AreEqual("key1", keys[0]);
            Assert.AreEqual("key2", keys[1]);
            Assert.AreEqual("key3", keys[2]);
        }
    }
}
