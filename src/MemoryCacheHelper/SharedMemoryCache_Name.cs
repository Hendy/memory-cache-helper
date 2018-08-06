namespace MemoryCacheHelper
{
    public sealed partial class SharedMemoryCache
    {
        /// <summary>
        /// The unique name of this memory cache
        /// </summary>
        public string Name => this._memoryCache.Name;
    }
}
