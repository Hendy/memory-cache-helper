using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class AddOrGetExistingTests : BaseTests
    {
        [TestMethod]
        public void AddOrGetExisting_String()
        {
            var input = "hello world";

            var output = MemoryCache.Instance.AddOrGetExisting<string>(KEY, () => input);

            Assert.AreEqual(input, output);
        }

        /// <summary>
        /// the first function to set a cache item should block any further functions on that key
        /// </summary>
        [TestMethod]
        public void Ensure_First_Function_Wins()
        {
            var output = "none";
            var started = false;

            Task.Run(() => {
                output = MemoryCache.Instance.AddOrGetExisting(KEY, () => {
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
                MemoryCache.Instance.AddOrGetExisting(KEY, () => "second");

                Assert.AreEqual("first", MemoryCache.Instance.Get<string>(KEY));
                Assert.AreEqual("first", output);
            }
        }
    }
}
