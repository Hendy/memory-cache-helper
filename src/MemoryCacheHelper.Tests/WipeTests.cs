﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            SharedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Empty_Wipe()
        {
            SharedMemoryCache.Instance.Wipe();

            Assert.IsTrue(SharedMemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Populated_Wipe()
        {
            TestHelper.Populate(1000);

            Assert.IsFalse(SharedMemoryCache.Instance.IsEmpty());

            SharedMemoryCache.Instance.Wipe();

            Assert.IsTrue(SharedMemoryCache.Instance.IsEmpty());
        }

        /// <summary>
        /// Perform a wipe, whilst a thread is populating the cache
        /// Whilst wiping, the set operations should be blocked
        /// The wipe should complete, and the set opertions resumed
        /// </summary>
        [TestMethod]
        public void Populating_Wipe()
        {
            Assert.IsTrue(SharedMemoryCache.Instance.IsEmpty());

            TestHelper.Populate(5000);

            Assert.IsFalse(SharedMemoryCache.Instance.IsEmpty());
            Assert.AreEqual(5000, SharedMemoryCache.Instance.GetKeys().Count());

            Parallel.Invoke(
                () => SharedMemoryCache.Instance.Wipe(), 
                () => TestHelper.Populate(1000));

            Assert.AreEqual(1000, SharedMemoryCache.Instance.GetKeys().Count());
        }
    }
}
