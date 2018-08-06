using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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
            SharedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Remove_Unknown_Key()
        {
            SharedMemoryCache.Instance.Remove("key");
        }

        [TestMethod]
        public void Remove_Null_Key()
        {
            SharedMemoryCache.Instance.Remove((string)null);
        }

        [TestMethod]
        public void Remove_Whitespace_Key()
        {
            SharedMemoryCache.Instance.Remove("");
        }

        [TestMethod]
        public void Remove_By_Function()
        {
            TestHelper.Populate(100, x => { return new KeyValuePair<string, object>("key" + x.ToString(), true); });

            Assert.IsFalse(SharedMemoryCache.Instance.IsEmpty());
            Assert.AreEqual(100, SharedMemoryCache.Instance.GetKeys().Count());

            SharedMemoryCache.Instance.Remove(x => x.StartsWith("key"));

            Assert.IsTrue(SharedMemoryCache.Instance.IsEmpty());
        }
    }
}
