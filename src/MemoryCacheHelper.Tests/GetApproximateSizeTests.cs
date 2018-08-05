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
            ExtendedMemoryCache.Instance.Wipe();
        }

        [TestMethod]
        public void Fill_And_See_GetApproxiateSize_Increase()
        {
            long oldSize = ExtendedMemoryCache.Instance.GetApproximateSize();
            long newSize = oldSize;
            
            if (SpinWait.SpinUntil(() => {

                ExtendedMemoryCache.Instance.Set(Guid.NewGuid().ToString(), DateTime.Now);

                newSize = ExtendedMemoryCache.Instance.GetApproximateSize();

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
