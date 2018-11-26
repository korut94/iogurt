using Iogurt.UI;
using UnityEngine;
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Menus;

namespace Iogurt
{
    public sealed class IogurtMainMenu : MonoBehaviour, IMenu, IConnectInterface
    {
        [SerializeField]
        AbstractAppsNavigator AppsNavigator;

        public Bounds localBounds { get; private set; }

        public MenuHideFlags menuHideFlags
        {
            get { return gameObject.activeSelf ? 0 : MenuHideFlags.Hidden; }
            set { gameObject.SetActive(value == 0); }
        }

        public GameObject menuContent { get { return gameObject; } }
        public AbstractAppsNavigator navigator { get { return AppsNavigator; } }
        public int priority { get { return 1; } }
    }
}

