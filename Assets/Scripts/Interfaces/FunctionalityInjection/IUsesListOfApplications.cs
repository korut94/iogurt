using System;
using System.Collections.Generic;

namespace Iogurt
{
    public interface IUsesListOfApplications
    {
        IEnumerable<Type> applications { set; }
    }
}

