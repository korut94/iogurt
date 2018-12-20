using Iogurt.Applications;
using UnityEngine;

namespace Iogurt.Tools
{
    [AppItem("Dummy")]
    public class DummyTool : MonoBehaviour, ITool, IConnectInterface, IInstantiateApplicationUI, IApplicationIcon
    {
        [SerializeField]
        DummyApp ApplicationPrefab;
        [SerializeField]
        Sprite Icon;

        GameObject m_application;

        public UI.Application application { get { return m_application.GetComponent<DummyApp>(); } }
        public Sprite icon { get { return Icon; } }

        void Start()
        {
            m_application = this.InstantiateApplicationUI(ApplicationPrefab);
            this.ConnectInterfaces(m_application);
        }
    }
}

