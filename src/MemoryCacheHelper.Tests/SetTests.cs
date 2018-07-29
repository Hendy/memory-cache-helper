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
            Thread innerThread = null;

            Task.Run(() =>
            {
                MemoryCache.Instance.AddOrGetExisting(KEY, () =>
                {
                    innerThread = new Thread(() =>
                    {
                        while (true) { };
                    });

                    innerThread.Start();
                    innerThread.Join();

                    return true;
                });

            });

            Thread.Sleep(1000);

            //Assert.IsTrue(innerThread != null && innerThread.IsAlive);

            Assert.IsNull(MemoryCache.Instance.Get<bool?>(KEY));

            MemoryCache.Instance.Set(KEY, false); // this should cancel the method waiting above

            Assert.IsFalse(MemoryCache.Instance.Get<bool?>(KEY).Value);

            Thread.Sleep(1000);
            //Assert.IsFalse(innerThread != null || innerThread.IsAlive);
            //Assert.IsFalse(innerThread.IsAlive);
        }
    }
}
