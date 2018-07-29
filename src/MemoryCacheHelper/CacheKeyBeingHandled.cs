using System.Threading;

namespace MemoryCacheHelper
{
    /// <summary>
    /// Respresents the setting state of a single cache item
    /// </summary>
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
