using DigitalRubyShared;
using Iogurt.Input.Touch;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class VerticalScrollView : MonoBehaviour, IUsesPanGesture
    {
        ScrollRect      m_scrollRect;
        float           m_startVerticalPosition;

        public void PanGesture(PanGestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Began)
            {
                m_startVerticalPosition = m_scrollRect.verticalNormalizedPosition;
            }
            else if (gesture.State == GestureRecognizerState.Executing)
            {
                var distance = BoundToWindowRect(gesture.DistanceY);
                m_scrollRect.verticalNormalizedPosition = m_startVerticalPosition - distance;
            }
        }

        float BoundToWindowRect(float distance)
        {
            return distance / Screen.height;
        }

        void Awake() 
        {
            m_scrollRect = GetComponent<ScrollRect>();
        }
    }
}
