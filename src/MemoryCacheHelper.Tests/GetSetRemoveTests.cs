using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class GetSetRemoveTests
    {
        private const string KEY = "exampleCacheKey";

        [TestInitialize]
        public void Initialize()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Get_String_Unknown_Key()
        {
            var found = false;

            var output = MemoryCache.Instance.Get<string>(KEY, out found);

            Assert.IsFalse(found);

            Assert.IsNull(output);
        }

        [TestMethod]
        public void Set_Then_Get_String()
        {
            var input = "hello world";

            MemoryCache.Instance.Set(KEY, input);

            var output = MemoryCache.Instance.Get<string>(KEY);

            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void GetSet_String()
        {
            var input = "hello world";

            var output = MemoryCache.Instance.GetSet<string>(KEY, () => input);

            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void Set_Type_Get_Different_Invalid_Type()
        {
            var input = true;

            MemoryCache.Instance.Set(KEY, input);

            var output = MemoryCache.Instance.Get<string>(KEY);

            Assert.IsNull(output);
        }

        [TestMethod]
        public void Remove_By_Lambda()
        {
            for(int i = 0; i < 10; i ++)
            {
                MemoryCache.Instance.Set(KEY + i.ToString(), true);
            }

            Assert.IsFalse(MemoryCache.Instance.IsEmpty());

            MemoryCache.Instance.Remove(x => x.StartsWith(KEY));

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }
    }
}
