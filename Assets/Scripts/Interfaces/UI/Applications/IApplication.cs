using UnityEngine;

namespace Iogurt.UI.Applications
{
    public interface IApplication
    {
        bool        isActivated { get; }
        GameObject  gameObject { get; }

        void Pause();
        void Resume();
    }
}

