﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryCacheHelper.Tests
{
    [TestClass]
    public class RemoveTests
    {
        /// <summary>
        /// Every test should start with an empty cache
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            MemoryCache.Instance.Wipe();

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }

        [TestMethod]
        public void Remove_Unknown_Key()
        {
            MemoryCache.Instance.Remove("key");
        }

        [TestMethod]
        public void Remove_By_Lambda()
        {
            for(int i = 0; i < 10; i ++)
            {
                var uniqueKey = "key" + i.ToString();

                MemoryCache.Instance.Set(uniqueKey, true);
            }

            Assert.IsFalse(MemoryCache.Instance.IsEmpty());

            MemoryCache.Instance.Remove(x => x.StartsWith("key"));

            Assert.IsTrue(MemoryCache.Instance.IsEmpty());
        }
    }
}
