using System.Collections;
using System.Collections.Generic;

using UnityButton = UnityEngine.UI.Button;

namespace Iogurt.UI
{
    public class Button : UnityButton, IWidget
    {
        public void Activate(bool active)
        {
            interactable = active;
        }
    }
}

