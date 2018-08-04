namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
    {
        /// <summary>
        /// Returns the current state
        /// </summary>
        /// <returns></returns>
        public bool IsWiping()
        {
            return this._isWiping;
        }
    }
}
