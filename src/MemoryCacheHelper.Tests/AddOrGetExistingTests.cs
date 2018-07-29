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
        /// the first function should block any others
        /// </summary>
        [TestMethod]
        public void Ensure_First_Function_Wins()
        {
            var output = false;
            var started = false;

            Task.Run(() => {
                output = MemoryCache.Instance.AddOrGetExisting(KEY, () => {
                    started = true;
                    Thread.Sleep(250);
                    return true;
                });
            });

            while (!started) { };
            //Thread.Sleep(50); // give the task enough time to make it's lock

            MemoryCache.Instance.AddOrGetExisting(KEY, () => false); // this should be blocked, so it's value not set

            Assert.IsTrue(MemoryCache.Instance.Get<bool>(KEY));  // expect the result from the task
            Assert.IsTrue(output);
        }
    }
}
