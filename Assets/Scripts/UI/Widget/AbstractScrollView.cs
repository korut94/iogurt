using DigitalRubyShared;
using Iogurt.Input.Touch;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class AbstractScrollView : AbstractWidget, IPanGesture
    {
        protected ScrollRect scrollRect { get; set; }

        void IPanGesture.OnPanGesture(PanGestureRecognizer gesture)
        {
            Debug.Log("Pan " + gesture.FocusX + " " + gesture.FocusY);
        }

        protected virtual void OnPanGesture(PanGestureRecognizer gesture) {}

        void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
        }
    }
}
