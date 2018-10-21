using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iogurt.Modules.Injection
{
    interface IInterfaceProvider
    {
        void ConnectInterface(object target, object userData = null);
        void DisconnectInterface(object target, object userData = null);
    }
}

