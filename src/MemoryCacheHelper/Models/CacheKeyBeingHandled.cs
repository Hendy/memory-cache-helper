using System;
using System.Threading;

namespace MemoryCacheHelper.Models
{
    /// <summary>
    /// Respresents the setting state of a cache item whilst a write operation is being attempted on it
    /// </summary>
    internal class CacheKeyBeingHandled
    {
        /// <summary>
        /// Locker for this cache key - only one thread should be attempting to set a specific cache item at any one time
        /// </summary>
        internal object Lock { get; set; } = new object();

        /// <summary>
        /// A thread running a function to set a cache item
        /// </summary>
        internal Thread Thread { get; set; }

        /// <summary>
        /// The return type of the function
        /// </summary>
        internal Type Type { get; set; }

        /// <summary>
        /// A directly set value (beating any function evaluation)
        /// </summary>
        internal object Value { get; set; }
    }
}
