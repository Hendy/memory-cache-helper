namespace MemoryCacheHelper
{
    public sealed partial class MemoryCache
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
