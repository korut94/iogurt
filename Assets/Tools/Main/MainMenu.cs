#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Menus;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;

namespace Iogurt.Tools
{
    public class MainMenu : MonoBehaviour, IMenu
    {
        public Action close;

        public Bounds       localBounds { get; private set; }
        public GameObject   menuContent { get { return gameObject; } }

        public MenuHideFlags menuHideFlags {
            get { return gameObject.activeSelf ? 0 : MenuHideFlags.Hidden; }
            set { gameObject.SetActive(value == 0); }
        }
        
        public int priority { get { return 1; } }

        public void Close()
        {
            Debug.Log("MainMenu is being closed");
            close();
        }

        void Awake()
        {
            localBounds = ObjectUtils.GetBounds(transform);
        }
    }
}

#endif
