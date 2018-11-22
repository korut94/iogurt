using UnityEngine;

namespace Iogurt
{
    public interface ITool
    {
        UI.Application application { get; }
    }

    public static class IToolMethods
    {
        public static bool HasRightSignature(this ITool obj) { return obj is MonoBehaviour; }
    }
}



