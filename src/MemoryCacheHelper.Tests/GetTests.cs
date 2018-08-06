using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class GetTests
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
        public void Get_Unknown_Key()
        {
            var found = false;

            var output = SharedMemoryCache.Instance.Get<string>("key", out found);

            Assert.IsFalse(found);

            Assert.IsNull(output);
        }

        [TestMethod]
        public void Get_String()
        {
            var input = "hello world";

            SharedMemoryCache.Instance.Set("key", input);

            var output = SharedMemoryCache.Instance.Get<string>("key");

            Assert.AreEqual(input, output);
        }
    }
}
