using DigitalRubyShared;

namespace Iogurt.Input.Touch
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPanGesture
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gesture"></param>
        void OnPanGesture(PanGestureRecognizer gesture);
    }
}

