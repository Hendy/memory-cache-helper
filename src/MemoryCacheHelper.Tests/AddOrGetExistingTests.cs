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
        public void GetAdd_String()
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
            Task.Run(() => {
                MemoryCache.Instance.AddOrGetExisting(KEY, () => {
                    Thread.Sleep(1000);
                    return true;
                });
            });

            Thread.Sleep(500); // give the task enough time to make it's lock

            MemoryCache.Instance.AddOrGetExisting(KEY, () => false); // this should be blocked, so it's value not set

            Assert.IsTrue(MemoryCache.Instance.Get<bool>(KEY));
        }
    }
}
