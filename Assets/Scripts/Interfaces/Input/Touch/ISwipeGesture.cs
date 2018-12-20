using DigitalRubyShared;

namespace Iogurt.Input.Touch
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISwipeGesture
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        void OnSwipeGesture(SwipeGestureRecognizerDirection direction, float speed);
    }
}

