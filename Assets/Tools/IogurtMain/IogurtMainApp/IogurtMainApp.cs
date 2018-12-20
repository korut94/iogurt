﻿using Iogurt.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.Applications
{
    public sealed class IogurtMainApp : UI.Application, IConnectInterface, IInstantiateApplicationUI
    {
        [SerializeField]
        Selectable Root;

        public override Selectable root { get { return Root; } }
    }
}

