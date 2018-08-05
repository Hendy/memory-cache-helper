using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace MemoryCacheHelper.Tests
{
    /// <summary>
    /// Testing the singleton
    /// </summary>
    [TestClass]
    public class InstanceTests
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

        [TestMethod]
        public void Ensure_Same_Instance_Between_Threads()
        {
            var thread1 = new Thread(() => {

                var instance = MemoryCache.Instance;

                instance.Set("key", true);

            });

            bool value = false;

            var thread2 = new Thread(() => {

                value = MemoryCache.Instance.Get<bool>("key");

            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Assert.IsTrue(value);            
        }

        [TestMethod]
        public void Ensure_Same_Instance_Between_Variables()
        {
            var variable1 = MemoryCache.Instance;
            var variable2 = MemoryCache.Instance;

            Assert.IsFalse(variable1.Get<bool>("key"));
            Assert.IsFalse(variable2.Get<bool>("key"));

            variable1.Set("key", true);

            Assert.IsTrue(variable1.Get<bool>("key"));
            Assert.IsTrue(variable2.Get<bool>("key"));
        }
    }
}
