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
            ExtendedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Remove_Unknown_Key()
        {
            ExtendedMemoryCache.Instance.Remove("key");
        }

        [TestMethod]
        public void Remove_Null_Key()
        {
            ExtendedMemoryCache.Instance.Remove((string)null);
        }

        [TestMethod]
        public void Remove_Whitespace_Key()
        {
            ExtendedMemoryCache.Instance.Remove("");
        }

        [TestMethod]
        public void Remove_By_Function()
        {
            TestHelper.Populate(100, x => { return new KeyValuePair<string, object>("key" + x.ToString(), true); });

            Assert.IsFalse(ExtendedMemoryCache.Instance.IsEmpty());
            Assert.AreEqual(100, ExtendedMemoryCache.Instance.GetKeys().Count());

            ExtendedMemoryCache.Instance.Remove(x => x.StartsWith("key"));

            Assert.IsTrue(ExtendedMemoryCache.Instance.IsEmpty());
        }
    }
}
