using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class WipeTests : BaseTests
    {
        [TestMethod]
        public void Empty_Wipe()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Populated_Wipe()
        {
            MemoryCache.Instance.Set(KEY, true);

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
            var setSomeItems = new Action<int>((count) => {
                for (int i = 0; i < count; i++)
                {
                    var key = Guid.NewGuid().ToString();
                    var value = DateTime.Now;

                    MemoryCache.Instance.Set(key, value);
                }
            });

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());

            setSomeItems(5000);

            Assert.IsFalse(MemoryCache.Instance.IsEmpty());
            Assert.AreEqual(5000, MemoryCache.Instance.GetKeys().Count());

            Parallel.Invoke(() => MemoryCache.Instance.Wipe(), () => setSomeItems(1000));

            Assert.AreEqual(1000, MemoryCache.Instance.GetKeys().Count());
        }    
    }
}
