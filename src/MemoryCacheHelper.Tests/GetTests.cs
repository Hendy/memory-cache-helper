using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class GetTests : BaseTests
    {
        [TestMethod]
        public void Get_Unknown_Key()
        {
            var found = false;

            var output = MemoryCache.Instance.Get<string>(KEY, out found);

            Assert.IsFalse(found);

            Assert.IsNull(output);
        }

        [TestMethod]
        public void Get_String()
        {
            var input = "hello world";

            MemoryCache.Instance.Set(KEY, input);

            var output = MemoryCache.Instance.Get<string>(KEY);

            Assert.AreEqual(input, output);
        }
    }
}
