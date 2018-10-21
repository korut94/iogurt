using Iogurt.UI.Applications;
using RSG;
using UnityEngine;

namespace Iogurt.UI
{
    public abstract class AbstractAppsNavigator : AbstractScrollView, IAppsNavigator
    {
        public abstract IPromise Next();
        public abstract IPromise Previous();
        public abstract IPromise SpawnApplication(GameObject app);
    }
}

