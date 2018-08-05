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
            ExtendedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Empty_Wipe()
        {
            ExtendedMemoryCache.Instance.Wipe();

            Assert.IsTrue(ExtendedMemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Populated_Wipe()
        {
            TestHelper.Populate(1000);

            Assert.IsFalse(ExtendedMemoryCache.Instance.IsEmpty());

            ExtendedMemoryCache.Instance.Wipe();

            Assert.IsTrue(ExtendedMemoryCache.Instance.IsEmpty());
        }

        /// <summary>
        /// Perform a wipe, whilst a thread is populating the cache
        /// Whilst wiping, the set operations should be blocked
        /// The wipe should complete, and the set opertions resumed
        /// </summary>
        [TestMethod]
        public void Populating_Wipe()
        {
            Assert.IsTrue(ExtendedMemoryCache.Instance.IsEmpty());

            TestHelper.Populate(5000);

            Assert.IsFalse(ExtendedMemoryCache.Instance.IsEmpty());
            Assert.AreEqual(5000, ExtendedMemoryCache.Instance.GetKeys().Count());

            Parallel.Invoke(
                () => ExtendedMemoryCache.Instance.Wipe(), 
                () => TestHelper.Populate(1000));

            Assert.AreEqual(1000, ExtendedMemoryCache.Instance.GetKeys().Count());
        }
    }
}
