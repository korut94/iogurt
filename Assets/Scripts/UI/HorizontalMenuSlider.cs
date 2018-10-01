using DigitalRubyShared;
using Iogurt.Input.Touch;
using UnityEngine;
using UnityEngine.UI;

namespace Iogurt.UI
{
    public sealed class HorizontalMenuSlider : MonoBehaviour, IUsesSwipeGesture
    {
        [SerializeField]
        MenuNavigator navigator;

        public void Swipe(SwipeGestureRecognizerDirection direction, float speed)
        {
            if (direction == SwipeGestureRecognizerDirection.Right)
            {
                navigator.Previous();
            }
            else if (direction == SwipeGestureRecognizerDirection.Left)
            {
                navigator.Next();
            }
        }
    }
}
