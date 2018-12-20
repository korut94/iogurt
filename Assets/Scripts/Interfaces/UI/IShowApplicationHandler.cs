using Iogurt.UI.Applications;
using RSG;

namespace Iogurt.UI
{
    public interface IShowApplicationHandler
    {
        IPromise ShowApplication(IApplication Prefab);
    }
}

