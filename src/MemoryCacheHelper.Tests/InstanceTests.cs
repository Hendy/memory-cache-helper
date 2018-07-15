using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class InstanceTests
    {
        private const string KEY = "exampleCacheKey";

        [TestInitialize]
        public void Initialize()
        {
            MemoryCacheHelper.Instance.Wipe();

            Assert.IsTrue(MemoryCacheHelper.Instance.IsEmpty());
        }

        [TestMethod]
        public void EnsureSameInstance_DifferentThreads()
        {
            var thread1 = new Thread(() => {

                var instance = MemoryCacheHelper.Instance;

                instance.Set(KEY, true);

            });

            bool value = false;

            var thread2 = new Thread(() => {

                value = MemoryCacheHelper.Instance.Get<bool>(KEY);

            });

            thread1.Start();
            thread1.Join();
            thread2.Start();
            thread2.Join();

            Assert.IsTrue(value);            
        }

        [TestMethod]
        public void EnsureSameInstance_DifferentVariables()
        {
            var variable1 = MemoryCacheHelper.Instance;
            var variable2 = MemoryCacheHelper.Instance;

            Assert.IsFalse(variable1.Get<bool>(KEY));
            Assert.IsFalse(variable2.Get<bool>(KEY));

            variable1.Set(KEY, true);

            Assert.IsTrue(variable1.Get<bool>(KEY));
            Assert.IsTrue(variable2.Get<bool>(KEY));
        }
    }
}
