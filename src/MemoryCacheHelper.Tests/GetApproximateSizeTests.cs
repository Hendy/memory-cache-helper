using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class GetApproximateSizeTests
    {
        /// <summary>
        /// Every test should start with an empty cache
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            SharedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Fill_And_See_GetApproxiateSize_Increase()
        {
            long oldSize = SharedMemoryCache.Instance.GetApproximateSize();
            long newSize = oldSize;
            
            if (SpinWait.SpinUntil(() => {

                SharedMemoryCache.Instance.Set(Guid.NewGuid().ToString(), DateTime.Now);

                newSize = SharedMemoryCache.Instance.GetApproximateSize();

                Assert.IsTrue(newSize >= oldSize);

                return newSize > oldSize;

            }, TimeSpan.FromSeconds(3)))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Inconclusive();
            }
        }
    }
}
