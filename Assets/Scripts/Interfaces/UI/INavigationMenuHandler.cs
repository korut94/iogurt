using RSG;

/// <summary>
/// 
/// </summary>
namespace Iogurt.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface INavigationMenuHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPromise Next();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPromise Previous();
    }
}
