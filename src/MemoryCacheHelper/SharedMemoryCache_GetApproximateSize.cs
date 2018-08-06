using System.Reflection;

namespace MemoryCacheHelper
{
    public sealed partial class SharedMemoryCache
    {
        /// <summary>
        /// https://stackoverflow.com/questions/22392634/how-to-measure-current-size-of-net-memory-cache-4-0
        /// </summary>
        /// <returns>Attempts to return the approximate size, otherwize returns -1 for unknown</returns>
        internal long GetApproximateSize()
        {
            long approximateSize;

            try
            {
                var statsField = typeof(System.Runtime.Caching.MemoryCache).GetField("_stats", BindingFlags.NonPublic | BindingFlags.Instance);
                var statsValue = statsField.GetValue(this._memoryCache);

                var monitorField = statsValue.GetType().GetField("_cacheMemoryMonitor", BindingFlags.NonPublic | BindingFlags.Instance);
                var monitorValue = monitorField.GetValue(statsValue);

                var sizeField = monitorValue.GetType().GetField("_sizedRefMultiple", BindingFlags.NonPublic | BindingFlags.Instance);
                var sizeValue = sizeField.GetValue(monitorValue);

                var approximateSizeProperty = sizeValue.GetType().GetProperty("ApproximateSize", BindingFlags.NonPublic | BindingFlags.Instance);

                approximateSize = (long)approximateSizeProperty.GetValue(sizeValue, null);
            }
            catch
            {
                approximateSize = -1;
            }

            return approximateSize;
        }
    }
}
