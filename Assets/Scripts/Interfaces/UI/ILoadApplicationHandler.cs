using Iogurt.UI.Applications;

namespace Iogurt.UI
{
    public interface ILoadApplicationHandler
    {
        IApplication LoadApplication(IApplication prefab);
    }
}
