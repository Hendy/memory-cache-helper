using System;
using System.Threading;

namespace MemoryCacheHelper.Models
{
    /// <summary>
    /// Respresents the setting state of a cache item whilst a write operation is being attempted on it via a consumer supplied function
    /// </summary>
    internal class CacheKeyBeingHandled
    {
        /// <summary>
        /// Locker for this cache key GetSet function
        /// </summary>
        internal object GetSetLock { get; set; } = new object();

        /// <summary>
        /// A thread running a function to GetSet a cache item
        /// </summary>
        internal Thread GetSetThread { get; set; }

        /// <summary>
        /// Locker for this cache key Set function
        /// </summary>
        internal object SetLock { get; set; } = new object();

        /// <summary>
        /// Used to count the number of threads being blocked whilst the last thread was being cancelled
        /// This allows us to skip a number of threads, and have just the last one make create a new thread for its function
        /// </summary>
        internal int SetThreadCounter { get; set; } = 0;

        /// <summary>
        /// A thread running a function to Set a cache item
        /// </summary>
        internal Thread SetThread { get; set; }

        /// <summary>
        /// The return type of the function
        /// </summary>
        internal Type GetSetType { get; set; }

        /// <summary>
        /// A directly set value (beating any function evaluation)
        /// </summary>
        internal object Value { get; set; }
    }
}
