using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MemoryCacheHelper.Tests
{
    internal static class TestHelper
    {
        /// <summary>
        /// Set the count number of items, where the key is guid + counter, and the value is the current datetime
        /// </summary>
        /// <param name="count"></param>
        internal static void Populate(int count)
        {
            TestHelper.Populate(count, (x) => {
                return new KeyValuePair<string, object>(
                    Guid.NewGuid().ToString() + "_" + x.ToString(), 
                    DateTime.Now);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="func"></param>
        internal static void Populate(int count, Func<int, KeyValuePair<string, object>> func)
        {
            for(var counter = 0; counter < count; counter ++)
            {
                var item = func(counter);

                ExtendedMemoryCache.Instance.Set(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Start a process to keep populating for the specified timespan duration.
        /// Each set attempt calls the supplied function for the key and value it should set
        /// </summary>
        /// <param name="func">function called for each set iteration</param>
        internal static void PopulateFor<T>(TimeSpan timeSpan, Func<int, KeyValuePair<string, object>> func)
        {
            Task.Run(() =>
            {
                var stopwatch = new Stopwatch();

                stopwatch.Start();

                int counter = 0;

                while (stopwatch.Elapsed <= timeSpan)
                {
                    counter++;

                    var item = func(counter);

                    ExtendedMemoryCache.Instance.Set(item.Key, item.Value);
                }
            });
        }
    }
}
