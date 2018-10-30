using DigitalRubyShared;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.UI
{
    public class VerticalScrollView : AbstractScrollView
    {
        float m_startVerticalPosition;

        protected override void OnPanGesture(PanGestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Began)
            {
                m_startVerticalPosition = scrollRect.verticalNormalizedPosition;
            }
            else if (gesture.State == GestureRecognizerState.Executing)
            {
                var distance = BoundToWindowRect(gesture.DistanceY);
                scrollRect.verticalNormalizedPosition = m_startVerticalPosition - distance;
            }
        }

        float BoundToWindowRect(float distance)
        {
            return distance / Screen.height;
        }
    }
}
