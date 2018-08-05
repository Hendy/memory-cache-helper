namespace MemoryCacheHelper
{
    public sealed partial class ExtendedMemoryCache
    {
        /// <summary>
        /// Returns the current state
        /// </summary>
        /// <returns>Returns true if the wipe operation is currently being executed</returns>
        internal bool IsWiping()
        {
            return this._isWiping;
        }
    }
}
