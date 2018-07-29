using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class RemoveTests : BaseTests
    {
        [TestMethod]
        public void Remove_Unknown_Key()
        {
            MemoryCache.Instance.Remove(KEY);
        }

        [TestMethod]
        public void Remove_By_Lambda()
        {
            for(int i = 0; i < 10; i ++)
            {
                var uniqueKey = KEY + i.ToString();

                MemoryCache.Instance.Set(uniqueKey, true);
            }

            Assert.IsFalse(MemoryCache.Instance.IsEmpty());

            MemoryCache.Instance.Remove(x => x.StartsWith(KEY));

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }
    }
}
