using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR;
using UnityEngine;

namespace Iogurt.Tools
{
    [MainMenuItem("Iogurt", "Extensions", "Starting Iogurt Editor Extension")]
    sealed class MainTool : MonoBehaviour, ITool, IConnectInterfaces, IInstantiateMenuUI,
        IUsesRayOrigin
    {
        [SerializeField]
        MainMenu m_menuPrefab;

        public Transform rayOrigin { get; set; }

        GameObject m_toolMenu;

        void Start() {
            // Clear selection so we can't manipulate things
            UnityEditor.Selection.activeGameObject = null;

            m_toolMenu = this.InstantiateMenuUI(rayOrigin, m_menuPrefab);
            var mainMenu = m_toolMenu.GetComponent<MainMenu>();

            this.ConnectInterfaces(mainMenu, rayOrigin);
        }
    }
}
