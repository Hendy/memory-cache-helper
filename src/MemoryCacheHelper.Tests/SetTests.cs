using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            Thread.Sleep(100); // allow time for the expensive func to start

            Assert.IsTrue(started);
            Assert.IsFalse(output.HasValue);
            Assert.IsFalse(MemoryCache.Instance.HasKey(KEY));

            MemoryCache.Instance.Set(KEY, true); // this should cancel the infinite loop method, triggering it's output to be set from this

            Assert.IsTrue(MemoryCache.Instance.HasKey(KEY));
            Assert.IsTrue(MemoryCache.Instance.Get<bool>(KEY));
            Assert.IsTrue(output.HasValue);
            Assert.IsTrue(output.Value);
        }
    }
}
