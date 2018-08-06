namespace MemoryCacheHelper.Models
{
    /// <summary>
    /// Respresents the setting state of a cache item whilst a write operation is being attempted on it via a consumer supplied function
    /// </summary>
    internal class CacheKeyBeingHandled
    {
        /// <summary>
        /// State of a GetGet operation acting on this key
        /// </summary>
        internal GetSetOperation GetSetOperation { get; } = new GetSetOperation();

        /// <summary>
        /// State of a Set operation acting on this key
        /// </summary>
        internal SetOperation SetOperation { get; } = new SetOperation();
    }
}
