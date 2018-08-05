using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            ExtendedMemoryCache.Instance.Wipe();
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
                output = ExtendedMemoryCache.Instance.AddOrGetExisting("key", () =>
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
                Assert.IsFalse(ExtendedMemoryCache.Instance.HasKey("key"));

                // immediately set the cache item, cancelling the infinite loop
                ExtendedMemoryCache.Instance.Set("key", "cancel");

                Assert.IsTrue(ExtendedMemoryCache.Instance.HasKey("key"));
                Assert.AreEqual("cancel", ExtendedMemoryCache.Instance.Get<string>("key"));

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

        [TestMethod]
        public void Last_Set_Function_Should_Win()
        {
            var slowSet = new Action<string>((x) =>
            {
                ExtendedMemoryCache.Instance.Set("key", () =>
                {
                    Thread.Sleep(500);
                    return x;
                });
            });

            Parallel.Invoke(
                () => slowSet("alpha"), 
                () => slowSet("bravo"),
                () => slowSet("charlie"),
                () => slowSet("delta"),
                () => slowSet("echo"),
                () => slowSet("foxtrot"),
                () => slowSet("golf"));

            ExtendedMemoryCache.Instance.Set("key", () => "last");

            Assert.AreEqual("last", ExtendedMemoryCache.Instance.Get<string>("key"));
        }
    }
}
