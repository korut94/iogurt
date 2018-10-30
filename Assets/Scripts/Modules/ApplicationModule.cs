using Iogurt.Modules.Injection;
using Iogurt.UI.Applications;
using RSG;
using System.Collections.Generic;
using UnityEngine;

namespace Iogurt.Modules
{
    public sealed class ApplicationModule : INested, IInterfaceProvider
    {
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
            IUsesCurrentApplicationMethods.currentApplication = CurrentApplication;
            IUsesCurrentApplicationMethods.currentApplicationIndex = CurrentApplicationIndex;
        }

        public void ConnectInterface(object target, object userData = null)
        {
            var listOfApplications = target as IUsesListOfApplications;
            if (listOfApplications != null)
            {
                listOfApplications.LoadedApplications = m_loadedApplications;
            }
        }

        public void DisconnectInterface(object target, object userData = null)
        {
            // var application = target as IApplication;
        }

        IApplication CurrentApplication() { return m_loadedApplications[m_currentApplicationIndex]; }

        int CurrentApplicationIndex() { return m_currentApplicationIndex; }

        GameObject InstantiateApplicationUI(IApplication prefab)
        {
            var go =  Object.Instantiate(prefab.gameObject);
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

