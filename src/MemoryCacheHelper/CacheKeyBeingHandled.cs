using System.Threading;

namespace MemoryCacheHelper
{
    internal class CacheKeyBeingHandled
    {
        /// <summary>
        /// Property exposes object to be used as a thread lock
        /// </summary>
        internal object Lock { get; set; } = new object();

        /// <summary>
        /// Handle to thread running consumer function
        /// </summary>
        internal Thread ExpensiveFunctionThread { get; set; }
    }
}
