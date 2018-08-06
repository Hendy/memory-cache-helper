namespace MemoryCacheHelper
{
    public sealed partial class SharedMemoryCache
    {
        /// <summary>
        /// Returns the current state
        /// </summary>
        /// <returns>Returns true if a non-abortable setting opertaion is taking place</returns>
        internal bool IsSetting()
        {
            return this._isSetting;
        }
    }
}
