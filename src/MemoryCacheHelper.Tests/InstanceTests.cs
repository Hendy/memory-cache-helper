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
            ExtendedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Ensure_Same_Instance_Between_Threads()
        {
            var thread1 = new Thread(() => {

                var instance = ExtendedMemoryCache.Instance;

                instance.Set("key", true);

            });

            bool value = false;

            var thread2 = new Thread(() => {

                value = ExtendedMemoryCache.Instance.Get<bool>("key");

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
            var variable1 = ExtendedMemoryCache.Instance;
            var variable2 = ExtendedMemoryCache.Instance;

            Assert.IsFalse(variable1.Get<bool>("key"));
            Assert.IsFalse(variable2.Get<bool>("key"));

            variable1.Set("key", true);

            Assert.IsTrue(variable1.Get<bool>("key"));
            Assert.IsTrue(variable2.Get<bool>("key"));
        }
    }
}
