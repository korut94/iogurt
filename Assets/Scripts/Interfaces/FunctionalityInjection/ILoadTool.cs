using System;

namespace Iogurt
{
    public interface ILoadTool
    {
    }

    public static class ILoadToolMethods
    {
        internal static Action<Type> loadTool { get; set; }

        public static void LoadTool(this ILoadTool obj, Type type)
        {
            loadTool(type);
        }
    }
}

