using UnityEngine;

namespace Iogurt.UI
{
    public interface ITouchScrollHandler
    {
        void OnTouchScroll(float speed, Vector2 direction);
    }
}

