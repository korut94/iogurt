using Iogurt.UI.Applications;
using System;
using UnityEngine;

namespace Iogurt
{
    public interface IInstantiateApplicationUI
    {
    }

    static class IInstantiateApplicationUIMethods
    {
        internal static Func<IApplication, GameObject> instantiateApplicationUI { get; set; }

        public static GameObject InstantiateApplicationUI(this IInstantiateApplicationUI obj, IApplication applicationPrefab)
        {
            return instantiateApplicationUI(applicationPrefab);
        }
    }
}

