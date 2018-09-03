using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Menus;
using UnityEngine;

namespace Iogurt.Tools
{
    public class MainMenu : MonoBehaviour, IMenu
    {
        public Bounds       localBounds { get; private set; }
        public GameObject   menuContent { get { return gameObject; } }

        public MenuHideFlags menuHideFlags {
            get { return gameObject.activeSelf ? 0 : MenuHideFlags.Hidden; }
            set { gameObject.SetActive(value == 0); }
        }
        
        public int priority { get { return 1; } }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}