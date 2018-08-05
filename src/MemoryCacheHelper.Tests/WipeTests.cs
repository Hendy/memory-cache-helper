using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class WipeTests
    {
        /// <summary>
        /// Every test should start with an empty cache
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Empty_Wipe()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Populated_Wipe()
        {
            MemoryCache.Instance.Set("key", true);

            Assert.IsFalse(MemoryCache.Instance.IsEmpty());

            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        /// <summary>
        /// Perform a wipe, whilst a thread is populating the cache
        /// Whilst wiping, the set operations should be blocked
        /// The wipe should complete, and the set opertions resumed
        /// </summary>
        [TestMethod]
        public void Populating_Wipe()
        {
            Assert.IsTrue(MemoryCache.Instance.IsEmpty());

            TestHelper.Populate(5000);

            Assert.IsFalse(MemoryCache.Instance.IsEmpty());
            Assert.AreEqual(5000, MemoryCache.Instance.GetKeys().Count());

            Parallel.Invoke(
                () => MemoryCache.Instance.Wipe(), 
                () => TestHelper.Populate(1000));

            Assert.AreEqual(1000, MemoryCache.Instance.GetKeys().Count());
        }
    }
}
