using RSG;
using UnityEngine;

namespace Iogurt.UI
{
    public abstract class MenuNavigator : MonoBehaviour, INavigationMenuHandler {
        public abstract IPromise Next();
        public abstract IPromise Previous();
    }
}

