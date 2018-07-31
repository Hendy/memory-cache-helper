using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
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

        [TestMethod]
        public void Populating_Wipe()
        {

        }

        /// <summary>
        /// Perform a wipe, whilst a long running function is executing
        /// </summary>
        [TestMethod]
        public void Wipe_Cancels_Infinite_Function()
        {
            var output = "none";
            var started = false;
            var finished = false;

            Task.Run(() =>
            {               
                output = MemoryCache.Instance.AddOrGetExisting(KEY, () =>
                {
                    started = true;

                    while (true) { }; // infinite loop

                    return "infinite";
                });

                finished = true;
            });

            if (!SpinWait.SpinUntil(() => started, 100))
            {
                Assert.Inconclusive("First call didn't start in time");
            }
            else
            {
                Assert.IsTrue(started);
                MemoryCache.Instance.Wipe();

                if (!SpinWait.SpinUntil(() => finished, 100))
                {
                    Assert.Inconclusive("First call didn't finish in time");
                }
                else
                {
                    Assert.IsNull(output);
                }
            }
        }

        public void Loads_Of_Data_Wipe()
        {

        }
    }
}
