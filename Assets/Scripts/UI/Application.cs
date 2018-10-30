using Iogurt.UI.Applications;
using UnityEngine;

namespace Iogurt.UI
{
    public abstract class Application : MonoBehaviour, IApplication
    {
        bool        m_isActivated = false;
        IWidget[]   m_widgets;

        public bool isActivated { get { return m_isActivated; } }

        public void Pause()
        {
            SetWidgetsState(false);
            m_isActivated = false;

            OnPause();
        }

        public void Resume()
        {
            SetWidgetsState(true);
            m_isActivated = true;

            OnResume();
        }

        protected virtual void OnResume() {}
        protected virtual void OnPause() {}

        void SetWidgetsState(bool active)
        {
            if (m_widgets == null)
                m_widgets = GetComponentsInChildren<IWidget>();

            foreach (var widget in m_widgets)
                widget.Activate(active);
        } 
    }
}
