using DigitalRubyShared;

namespace Iogurt.Input.Touch
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUsesSwipeGesture
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        void Swipe(SwipeGestureRecognizerDirection direction, float speed);
    }
}

