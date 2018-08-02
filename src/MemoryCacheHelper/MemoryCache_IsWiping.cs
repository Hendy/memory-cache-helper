namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Returns the current state
        /// </summary>
        /// <returns></returns>
        internal bool IsWiping()
        {
            return this._isWiping;
        }
    }
}
