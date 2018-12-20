using Iogurt.Applications;
using UnityEngine;

namespace Iogurt.Tools
{
    [AppItem("Create Primitive")]
    public class SpawnObjectsTool : MonoBehaviour, ITool, IConnectInterface, IInstantiateApplicationUI, IApplicationIcon
    {
        [SerializeField]
        SpawnObjectsApp ApplicationPrefab;
        [SerializeField]
        Sprite Icon;

        GameObject m_application;

        public UI.Application application { get { return m_application.GetComponent<SpawnObjectsApp>(); } }
        public Sprite icon { get { return Icon; } }

        void Start()
        {
            m_application = this.InstantiateApplicationUI(ApplicationPrefab);
            this.ConnectInterfaces(m_application);
	    }
    }
}
