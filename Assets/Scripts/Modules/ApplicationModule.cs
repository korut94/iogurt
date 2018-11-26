using Iogurt.Modules.Injection;
using Iogurt.UI.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.EditorVR.Utilities;

using UnityObject = UnityEngine.Object;

namespace Iogurt.Modules
{
    public sealed class ApplicationModule : INested, IInterfaceProvider
    {
        Dictionary<Type, MonoBehaviour> m_availableTools = new Dictionary<Type, MonoBehaviour>();

        int                 m_currentApplicationIndex = -1;
        List<IApplication>  m_loadedApplications = new List<IApplication>();
        IAppsNavigator      m_navigator;

        public IAppsNavigator navigator
        {
            set { m_navigator = value; }
        }

        public ApplicationModule()
        {
            IInstantiateApplicationUIMethods.instantiateApplicationUI = InstantiateApplicationUI;
            ILoadToolMethods.loadTool = LoadTool;
            IUsesCurrentApplicationMethods.currentApplication = CurrentApplication;
            IUsesCurrentApplicationMethods.currentApplicationIndex = CurrentApplicationIndex;
        }

        public void ConnectInterface(object target, object userData = null)
        {
            var loadedApplications = target as IUsesLoadedApplications;
            if (loadedApplications != null)
                loadedApplications.LoadedApplications = m_loadedApplications;

            var listOfApplications = target as IUsesListOfApplications;
            if (listOfApplications != null)
                listOfApplications.applications = m_availableTools.Select(pair => pair.Key);

            var tool = target as ITool;
            if (tool != null && tool.HasRightSignature())
            {
                if (!m_availableTools.ContainsKey(target.GetType()))
                {
                    m_availableTools[target.GetType()] = target as MonoBehaviour;
                }
            }
        }

        public void DisconnectInterface(object target, object userData = null)
        {
            // var application = target as IApplication;
        }

        public bool IsAvailable(Type tool)
        {
            return m_availableTools.ContainsKey(tool);
        }

        IApplication CurrentApplication() { return m_loadedApplications[m_currentApplicationIndex]; }

        int CurrentApplicationIndex() { return m_currentApplicationIndex; }

        GameObject InstantiateApplicationUI(IApplication prefab)
        {
            var go = ObjectUtils.Instantiate(prefab.gameObject, m_navigator.content, false);
            var app = go.GetComponent<IApplication>();

            m_loadedApplications.Add(app);

            if (m_currentApplicationIndex > -1)
                m_loadedApplications[m_currentApplicationIndex].Pause();

            m_currentApplicationIndex++;

            m_navigator
                .SpawnApplication(go)
                .Then(() => app.Resume());

            return go;
        }

        void LoadTool(Type type)
        {
            MonoBehaviour behaviour;

            if (m_availableTools.TryGetValue(type, out behaviour))
            {
                behaviour.enabled = true;
            }
        }
    }
}

