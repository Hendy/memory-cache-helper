using System.Threading;

namespace MemoryCacheHelper.Models
{
    internal class SetOperation
    {
        /// <summary>
        /// Locker for this cache key Set function
        /// </summary>
        internal object Lock { get; } = new object();

        /// <summary>
        /// Used to count the number of threads being blocked whilst the last thread is being cancelled
        /// </summary>
        internal int Counter { get; set; }

        /// <summary>
        /// A thread running a function to Set a cache item
        /// </summary>
        internal Thread Thread { get; set; }
    }
}
