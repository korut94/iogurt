using Iogurt.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.Applications
{
    public sealed class SpawnObjectsApp : UI.Application, IConnectInterface
    {
        [SerializeField]
        Selectable Root;

        public override Selectable root { get { return Root; } }
    }
}