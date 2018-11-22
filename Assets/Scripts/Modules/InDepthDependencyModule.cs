using Iogurt.UI;
using System.Linq;
using UnityEngine;

namespace Iogurt.Modules.Injection
{
    public sealed class InDepthDependencyModule : INested, IInterfaceProvider, IConnectInterface
    {
        public void ConnectInterface(object target, object userData = null)
        {
            var go = target as GameObject;
            if (go != null)
            {
                foreach (var widget in go.GetComponentsInChildren<IWidget>(true))
                {
                    this.ConnectInterfaces(widget);
                }
            }
        }

        public void DisconnectInterface(object target, object userData = null)
        {
            // throw new System.NotImplementedException();
        }
    }
}

