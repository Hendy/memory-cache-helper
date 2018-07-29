using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

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
            MemoryCache.Instance.AddOrGetExisting(KEY, () => {
                Thread.Sleep(System.TimeSpan.FromSeconds(5).Milliseconds);
                return true;
            });

            MemoryCache.Instance.AddOrGetExisting(KEY, () => false);

            Assert.IsTrue(MemoryCache.Instance.Get<bool>(KEY));
        }
    }
}
