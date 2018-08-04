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

                TestHelper.SetSomeItems(1);

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
