using Iogurt.UI.Applications;
using System.Collections.Generic;

namespace Iogurt
{
    public interface IUsesListOfApplications
    {
        IList<IApplication> LoadedApplications { set; }
    }
}

