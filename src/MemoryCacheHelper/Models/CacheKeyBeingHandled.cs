using System;
using System.Threading;

namespace MemoryCacheHelper.Models
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
        internal Thread Thread { get; set; }

        /// <summary>
        /// The return type of the value function
        /// </summary>
        internal Type Type { get; set; }

        /// <summary>
        /// The directly set value (beating any function evaluation)
        /// </summary>
        internal object Value { get; set; }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="type"></param>
        //internal CacheKeyBeingHandled(Type type)
        //{
        //    this.Type = type;
        //}
    }
}
