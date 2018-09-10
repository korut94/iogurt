using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iogurt.UI
{
    public interface ITouchHandler
    {
        void OnTouch(Vector2 relativePosition);
    }
}

