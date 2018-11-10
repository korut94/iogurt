using Iogurt.Modules.Injection;
using Iogurt.UI.Applications;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Iogurt.Modules
{
    public sealed class ApplicationModule : INested, IInterfaceProvider
    {
        IEnumerable<Type>   m_availableApplications;
        int                 m_currentApplicationIndex = -1;
        List<IApplication>  m_loadedApplications = new List<IApplication>();
        IAppsNavigator      m_navigator;

        public IEnumerable<Type> availableApplications
        {
            set { m_availableApplications = value; }
        }

        public IAppsNavigator navigator
        {
            set { m_navigator = value; }
        }

        public ApplicationModule()
        {
            IInstantiateApplicationUIMethods.instantiateApplicationUI = InstantiateApplicationUI;
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
                listOfApplications.applications = m_availableApplications;
        }

        public void DisconnectInterface(object target, object userData = null)
        {
            // var application = target as IApplication;
        }

        IApplication CurrentApplication() { return m_loadedApplications[m_currentApplicationIndex]; }

        int CurrentApplicationIndex() { return m_currentApplicationIndex; }

        GameObject InstantiateApplicationUI(IApplication prefab)
        {
            var go =  UnityObject.Instantiate(prefab.gameObject);
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
    }
}

