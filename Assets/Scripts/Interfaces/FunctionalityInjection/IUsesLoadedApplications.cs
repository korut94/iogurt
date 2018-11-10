using Iogurt.UI.Applications;
using System.Collections.Generic;

namespace Iogurt
{
    public interface IUsesLoadedApplications
    {
        IList<IApplication> LoadedApplications { set; }
    }
}

