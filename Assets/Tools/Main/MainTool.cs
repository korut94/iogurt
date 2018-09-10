#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR;
using UnityEngine;
using UnityEngine.InputNew;

namespace Iogurt.Tools
{
    [MainMenuItem("Iogurt", "Extensions", "Starting Iogurt Editor Extension")]
    sealed class MainTool : MonoBehaviour, ITool, IConnectInterfaces, IInstantiateMenuUI,
        IUsesRayOrigin, ICustomActionMap
    {
        [SerializeField]
        ActionMap m_actionMap;   
        [SerializeField]
        MainMenu m_menuPrefab;

        public ActionMap    actionMap { get { return m_actionMap; } }
        public bool         ignoreLocking { get { return false; } }
        public Transform    rayOrigin { get; set; }

        GameObject m_toolMenu;

        public void ProcessInput(ActionMapInput input, ConsumeControlDelegate consumeControl)
        {
            var mainMenuInput = (MainToolInput) input;
            var touchX = mainMenuInput.touchXAxis;
            var touchY = mainMenuInput.touchYAxis;

            Debug.Log(new Vector2(touchX.value, touchY.value));
        }


        void Start() {
            // Clear selection so we can't manipulate things
            UnityEditor.Selection.activeGameObject = null;

            m_toolMenu = this.InstantiateMenuUI(rayOrigin, m_menuPrefab);
            var mainMenu = m_toolMenu.GetComponent<MainMenu>();

            this.ConnectInterfaces(mainMenu, rayOrigin);
        }
    }
}
#endif
