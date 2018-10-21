using System;
using System.Collections.Generic;

namespace Iogurt.Modules.Injection
{
    public sealed class InterfaceBinder : INested
    {
        readonly HashSet<object> m_connectedInterfaces = new HashSet<object>();

        event Action<object, object> m_onConnectInterface;
        event Action<object, object> m_onDisconnectInterface;

        public InterfaceBinder()
        {
            IConnectInterfaceMethods.connectInterfaces = ConnectInterface;
            IConnectInterfaceMethods.disconnectInterfaces = DisconnectInterface;
        }

        public void AttachInterfaceProvider(object target)
        {
            var provider = target as IInterfaceProvider;

            if (provider != null)
            {
                m_onConnectInterface += provider.ConnectInterface;
                m_onDisconnectInterface += provider.DisconnectInterface;
            }
        }

        void ConnectInterface(object target, object userData = null)
        {
            if (!m_connectedInterfaces.Add(target))
                return;

            if (m_onConnectInterface != null)
                m_onConnectInterface(target, userData);
        }

        void DisconnectInterface(object target, object userData = null)
        {
            m_connectedInterfaces.Remove(target);

            if (m_onDisconnectInterface != null)
                m_onDisconnectInterface(target, userData);
        }
    }
}

