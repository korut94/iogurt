using Iogurt.UI;

namespace Iogurt.Applications
{
    public sealed class IogurtMainApp : Application, IConnectInterface, IInstantiateApplicationUI
    {
        public void CreateAnOtherMenu()
        {
            var go = this.InstantiateApplicationUI(this);
            var app = go.GetComponent<IogurtMainApp>();

            this.ConnectInterfaces(app);
        }
    }
}

