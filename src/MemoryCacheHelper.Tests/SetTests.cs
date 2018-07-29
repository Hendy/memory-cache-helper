using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class SetTests : BaseTests
    {
        /// <summary>
        /// A direct set should trigger any function working on this key should be cancelled
        /// </summary>
        [TestMethod]
        public void Cancel_Long_Running_Function()
        {
            var output = false;
            var started = false;

            Task.Run(() =>
            {
                output = MemoryCache.Instance.AddOrGetExisting(KEY, () =>
                {
                    started = true;

                    while (true) { };

                    return true;
                });

            });

            Thread.Sleep(500); // allow the expensive func to start

            Assert.IsTrue(started);

            Assert.IsFalse(MemoryCache.Instance.HasKey(KEY));

            MemoryCache.Instance.Set(KEY, false); // this should cancel the infinite loop method, triggering it's output to be set from this

            Assert.IsTrue(MemoryCache.Instance.HasKey(KEY));
            Assert.IsFalse(MemoryCache.Instance.Get<bool>(KEY));

            Assert.IsFalse(output);
        }
    }
}
