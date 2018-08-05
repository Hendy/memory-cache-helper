using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class SetTests
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

        /// <summary>
        /// A direct set should trigger any function working on this key should be cancelled
        /// </summary>
        [TestMethod]
        public void Cancel_Long_Running_Function()
        {
            var output = "none";
            var started = false;
            var finished = false;

            Task.Run(() =>
            {
                output = MemoryCache.Instance.AddOrGetExisting("key", () =>
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

                // there shouldn't be an item, as the infinite loop prevents the set
                Assert.IsFalse(MemoryCache.Instance.HasKey("key"));

                // immediately set the cache item, cancelling the infinite loop
                MemoryCache.Instance.Set("key", "cancel");

                Assert.IsTrue(MemoryCache.Instance.HasKey("key"));
                Assert.AreEqual("cancel", MemoryCache.Instance.Get<string>("key"));

                // wait for the cancelled task to finish
                if (!SpinWait.SpinUntil(() => finished, 100))
                {
                    Assert.Inconclusive("First call didn't finish in time");
                }
                else
                {
                    Assert.AreEqual("cancel", output);
                }
            }
        }
    }
}
