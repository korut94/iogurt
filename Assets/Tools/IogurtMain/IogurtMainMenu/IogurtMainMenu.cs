using Iogurt.UI;
using UnityEngine;

namespace Iogurt
{
    public sealed class IogurtMainMenu : MonoBehaviour, IConnectInterface
    {
        [SerializeField]
        AbstractAppsNavigator AppsNavigator;

        public AbstractAppsNavigator navigator { get { return AppsNavigator; } }
    }
}

