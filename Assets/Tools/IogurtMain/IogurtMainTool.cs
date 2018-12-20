using Iogurt.Applications;
using Iogurt.Modules;
using Iogurt.Modules.Injection;
// using Iogurt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputNew;
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Utilities;

using EVR = UnityEditor.Experimental.EditorVR;

namespace Iogurt.Tools
{ 
    [MainMenuItem("Iogurt", "Plugins", "Starting Iogurt Editor Extension")]
    public sealed class IogurtMainTool : MonoBehaviour,
        EVR.ITool, EVR.IConnectInterfaces, EVR.ICustomActionMap, EVR.IUsesRayOrigin, EVR.IUsesNode, EVR.IInstantiateMenuUI,
        ITool, IConnectInterface, IInstantiateApplicationUI
    {
        [SerializeField]
        IogurtMainApp ApplicationPrefab;
        [SerializeField]
        IogurtMainMenu MenuPrefab;
        [SerializeField]
        ActionMap ActionMap;

        InterfaceBinder m_binder;

        Dictionary<Type, INested> m_nestedModules = new Dictionary<Type, INested>();
        Dictionary<Type, MonoBehaviour> m_modules = new Dictionary<Type, MonoBehaviour>();

        GameObject  m_application;
        GameObject  m_menu;

        public ActionMap        actionMap { get { return ActionMap; } }
        public UI.Application   application { get { return m_application.GetComponent<IogurtMainApp>(); } }
        public bool             ignoreLocking { get { return false; } }
        public Transform        rayOrigin { get; set; }
        public Node             node { get; set; }

        void Start()
        {
            // Nested Module initialization
            m_binder = AddNestedModule<InterfaceBinder>();
            AddNestedModule<InDepthDependencyModule>();
            AddNestedModule<ApplicationDataModule>();
            var applicationModule = AddNestedModule<ApplicationModule>();
            // Module initialization
            AddModule<TouchInputModule>();

            // Menu initialization
            m_menu = this.InstantiateMenuUI(rayOrigin, MenuPrefab);
            var menu = m_menu.GetComponent<IogurtMainMenu>();
            this.ConnectInterfaces(m_menu);

            applicationModule.navigator = menu;

            var tools = ObjectUtils.GetImplementationsOfInterface(typeof(ITool)).Where(type => type != typeof(IogurtMainTool));
            foreach (var tool in tools)
                if (!applicationModule.IsAvailable(tool))
                    AddTool(tool);
            
            // Application initialization
            m_application = this.InstantiateApplicationUI(ApplicationPrefab);
            this.ConnectInterfaces(m_application);
        }

        public void ProcessInput(ActionMapInput input, ConsumeControlDelegate consumeControl)
        {
            var customInput = input as IogurtMainInput;

            GetModule<TouchInputModule>().UpdateAxes(node, customInput.touchXAxis, customInput.touchYAxis);

            consumeControl(customInput.touchXAxis);
            consumeControl(customInput.touchYAxis);
        }

        T AddModule<T>() where T : MonoBehaviour
        {
            MonoBehaviour module;
            var type = typeof(T);

            if (!m_modules.TryGetValue(type, out module))
            {
                module = ObjectUtils.AddComponent<T>(gameObject);
                m_modules.Add(type, module);

                // May be a late binding is required in the case some nested modules need
                // to use the just loaded module

                this.ConnectInterfaces(module);
                m_binder.AttachInterfaceProvider(module);
            }

            return (T) module;
        }

        T GetModule<T>() where T : MonoBehaviour
        {
            MonoBehaviour module;
            m_modules.TryGetValue(typeof(T), out module);
            return (T) module;
        }

        T AddNestedModule<T>() where T : INested
        {
            INested nested;
            var type = typeof(T);

            // There's must be just one instance for each type of module
            if (!m_nestedModules.TryGetValue(type, out nested))
            {
                nested = Activator.CreateInstance(type) as INested;
                m_nestedModules.Add(type, nested);

                // Note: InterfaceBinder is not attach to any other module since when AddNestedModule
                // is invoke with InterfaceBinder the m_binder points to the null referece.
                if (m_binder != null)
                {
                    // The already register modules can supply to the new one our own dependencies. In
                    // this way the modules can interact with each other without having an explicit pointer.
                    // The connection event must be emitted before the real attachment otherwise the
                    // new nested module will find to manage itself.
                    this.ConnectInterfaces(nested);
                    m_binder.AttachInterfaceProvider(nested);
                }
            }

            return (T) nested;
        }

        T GetNestedModule<T>() where T : INested
        {
            return (T) m_nestedModules[typeof(T)];
        }

        MonoBehaviour AddTool(Type type) 
        {
            MonoBehaviour behaviour = ObjectUtils.AddComponent(type, gameObject) as MonoBehaviour;
            behaviour.enabled = false;

            this.ConnectInterfaces(behaviour);
            return behaviour;
        }
    }
}
