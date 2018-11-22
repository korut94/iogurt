﻿using Iogurt.Applications;
using Iogurt.Modules;
using Iogurt.Modules.Injection;
using Iogurt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Iogurt.Tools
{
    sealed class IogurtMainTool : MonoBehaviour, ITool, IConnectInterface, IInstantiateApplicationUI
    {
        [SerializeField]
        IogurtMainApp   ApplicationPrefab;
        [SerializeField]
        IogurtMainMenu  MenuPrefab;

        InterfaceBinder m_binder;

        Dictionary<Type, INested> m_nestedModules = new Dictionary<Type, INested>();

        GameObject  m_application;
        GameObject  m_menu;

        public UI.Application application { get { return m_application.GetComponent<IogurtMainApp>(); } }

        // Use this for initialization
        void Awake()
        {
            // Module initialization
            m_binder = AddNestedModule<InterfaceBinder>();
            AddNestedModule<InDepthDependencyModule>();
            AddNestedModule<ApplicationDataModule>();
            var applicationModule = AddNestedModule<ApplicationModule>();
            
            // Menu initialization
            var menu = Instantiate(MenuPrefab, transform);
            m_menu = menu.gameObject;

            this.ConnectInterfaces(m_menu);

            applicationModule.navigator = menu.navigator;

            var tools = ObjectUtils.GetImplementationsOfInterface(typeof(ITool)).Where(type => type != typeof(IogurtMainTool));
            foreach (var tool in tools)
                if (!applicationModule.IsAvailable(tool))
                    AddTool(tool);
            
            // Application initialization
            m_application = this.InstantiateApplicationUI(ApplicationPrefab);
            this.ConnectInterfaces(m_application);
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

        MonoBehaviour AddTool(Type type) 
        {
            MonoBehaviour behaviour = gameObject.AddComponent(type) as MonoBehaviour;
            behaviour.enabled = false;

            this.ConnectInterfaces(behaviour);
            return behaviour;
        }
    }
}
