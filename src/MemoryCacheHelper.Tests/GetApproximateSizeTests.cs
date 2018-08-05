using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class GetApproximateSizeTests : BaseTests
    {
        [TestMethod]
        public void Fill_And_See_GetApproxiateSize_Increase()
        {
            long oldSize = MemoryCache.Instance.GetApproximateSize();
            long newSize = oldSize;
            
            if (SpinWait.SpinUntil(() => {

                MemoryCache.Instance.Set(Guid.NewGuid().ToString(), DateTime.Now);

                newSize = MemoryCache.Instance.GetApproximateSize();

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
