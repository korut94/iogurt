using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iogurt.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITooltipHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tooltip"></param>
        void CloseTooltip(GameObject tooltip);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        GameObject ShowTooltip(GameObject tooltip);
    }
}

