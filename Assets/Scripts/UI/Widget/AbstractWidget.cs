using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iogurt.UI
{
    public abstract class AbstractWidget : MonoBehaviour, IWidget
    {
        bool m_activate = true;

        public bool activate
        {
            get { return m_activate; }
            set { Activate(value); }
        }

        public void Activate(bool active = true)
        {
            m_activate = active;
        }
    }
}

