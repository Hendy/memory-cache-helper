using System;
using System.Threading;

namespace MemoryCacheHelper.Models
{
    internal class GetSetOperation
    {
        /// <summary>
        /// Locker for this cache key GetSet function
        /// </summary>
        internal object Lock { get; } = new object();

        /// <summary>
        /// A thread running a function to GetSet a cache item
        /// </summary>
        internal Thread Thread { get; set; }

        /// <summary>
        /// The return type of the GetSet function
        /// </summary>
        internal Type Type { get; set; }

        /// <summary>
        /// A directly set value (beating any function evaluation)
        /// </summary>
        internal object Value { get; set; }
    }
}
