using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        /// Perform a wipe, whilst a long running function is setting a set
        /// </summary>
        [TestMethod]
        public void Populating_Wipe()
        {
            bool? output = null;
            var started = false;

            Task.Run(() =>
            {
                output = MemoryCache.Instance.AddOrGetExisting(KEY, () =>
                {
                    started = true;

                    while (true) { }; // infinite loop

                    return false;
                });

            });

            while (!started) { }

            Assert.IsTrue(started);

            MemoryCache.Instance.Wipe();

            // how do we see if the concurrent dictionary is empty too ?
            //Assert.

        }
    }
}
