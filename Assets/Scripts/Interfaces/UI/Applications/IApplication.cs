using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.UI.Applications
{
    public interface IApplication
    {
        bool        isActivated { get; }
        GameObject  gameObject { get; }
        Selectable  root { get; }

        void Pause();
        void Resume();
    }
}

