using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace MemoryCacheHelper.Tests
{
    /// <summary>
    /// Testing the singleton
    /// </summary>
    [TestClass]
    public class InstanceTests : BaseTests
    {
        [TestMethod]
        public void Ensure_Same_Instance_Between_Threads()
        {
            var thread1 = new Thread(() => {

                var instance = MemoryCache.Instance;

                instance.Set(KEY, true);

            });

            bool value = false;

            var thread2 = new Thread(() => {

                value = MemoryCache.Instance.Get<bool>(KEY);

            });

            thread1.Start();
            thread1.Join();
            thread2.Start();
            thread2.Join();

            Assert.IsTrue(value);            
        }

        [TestMethod]
        public void Ensure_Same_Instance_Between_Variables()
        {
            var variable1 = MemoryCache.Instance;
            var variable2 = MemoryCache.Instance;

            Assert.IsFalse(variable1.Get<bool>(KEY));
            Assert.IsFalse(variable2.Get<bool>(KEY));

            variable1.Set(KEY, true);

            Assert.IsTrue(variable1.Get<bool>(KEY));
            Assert.IsTrue(variable2.Get<bool>(KEY));
        }
    }
}
