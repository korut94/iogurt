using System;

namespace Iogurt
{
    public interface IConnectInterface
    {
    }

    static class IConnectInterfaceMethods
    {
        internal static Action<object, object> connectInterfaces { get; set; }
        internal static Action<object, object> disconnectInterfaces { get; set; }

        public static void ConnectInterfaces(this IConnectInterface obj, object target, object userData = null)
        {
            connectInterfaces(target, userData);
        }

        public static void DisconnectInterfaces(this IConnectInterface obj, object target, object userData = null)
        {
            disconnectInterfaces(target, userData);
        }
    }
}

