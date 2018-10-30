using Iogurt.UI.Applications;
using System;

namespace Iogurt
{
    public interface IUsesCurrentApplication
    {
    }

    public static class IUsesCurrentApplicationMethods
    {
        internal static Func<IApplication> currentApplication { get; set; }
        internal static Func<int> currentApplicationIndex { get; set; }

        public static IApplication CurrentApplication(this IUsesCurrentApplication obj)
        {
            return currentApplication();
        }

        public static int CurrentApplicationIndex(this IUsesCurrentApplication obj)
        {
            return currentApplicationIndex();
        }
    }
}
