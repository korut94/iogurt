using RSG;
using UnityEngine;

namespace Iogurt.UI.Applications
{                
    /// <summary>
    /// 
    /// </summary>
    public interface IAppsNavigator
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPromise SpawnApplication(GameObject app); 
    }
}

