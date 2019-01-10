using System;
using UnityEngine;

namespace Iogurt
{
    public interface IUsesTooltip
    {
    }

    public static class IUsesTooltipMethods
    {
        internal static Func<GameObject, GameObject> showTooltip { get; set; }
        internal static Action<GameObject> closeTooltip { get; set; }

        public static GameObject ShowTooltip(this IUsesTooltip obj, GameObject prefab)
        {
            return showTooltip(prefab);
        }

        public static void CloseTooltip(this IUsesTooltip obj, GameObject tooltip)
        {
            closeTooltip(tooltip);
        }
    }
}

