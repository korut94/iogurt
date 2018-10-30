using DigitalRubyShared;
using Iogurt.Input.Touch;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class AbstractScrollView : AbstractWidget, IUsesPanGesture
    {
        private ScrollRect m_scrollRect;

        protected ScrollRect scrollRect { get { return m_scrollRect; } }

        public void PanGesture(PanGestureRecognizer gesture)
        {
            if (activate)
                OnPanGesture(gesture);
        }

        protected virtual void OnPanGesture(PanGestureRecognizer gesture) {}

        void Awake()
        {
            m_scrollRect = GetComponent<ScrollRect>();
        }
    }
}
