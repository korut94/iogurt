using System;
using UnityEngine;

namespace Iogurt
{
    public interface IUsesApplicationIcon
    {
    }

    public static class IUsesApplicationIconMethods
    {
        internal static Func<Type, Sprite> getApplicationIcon { get; set; } 

        public static Sprite GetApplicationIcon(this IUsesApplicationIcon obj, Type type)
        {
            return getApplicationIcon(type);
        }
    }
}

