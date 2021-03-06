﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class AddOrGetExistingTests
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
        public void AddOrGetExisting_String()
        {
            var input = "hello world";

            var output = SharedMemoryCache.Instance.AddOrGetExisting<string>("key", () => input);

            Assert.AreEqual(input, output);
        }

        /// <summary>
        /// the first function to attempt to add a cache item, should block any further functions on that key
        /// </summary>
        [TestMethod]
        public void Ensure_First_Function_Wins()
        {
            var output = "none";
            var started = false;

            Task.Run(() => {
                output = SharedMemoryCache.Instance.AddOrGetExisting("key", () => {
                    started = true;
                    Thread.Sleep(250);
                    return "first";
                });
            });

            if (! SpinWait.SpinUntil(() => started, 100))
            {
                Assert.Inconclusive("First call didn't start in time");
            }
            else
            {
                Assert.IsTrue(started);

                // this should be blocked, so it's value should not be set
                SharedMemoryCache.Instance.AddOrGetExisting("key", () => "second");

                Assert.AreEqual("first", SharedMemoryCache.Instance.Get<string>("key"));
                Assert.AreEqual("first", output);
            }
        }
    }
}
