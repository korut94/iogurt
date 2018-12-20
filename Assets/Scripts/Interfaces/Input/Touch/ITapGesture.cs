using DigitalRubyShared;

namespace Iogurt.Input.Touch
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITapGesture
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gesture"></param>
        void OnTapGesture(TapGestureRecognizer gesture);
    }
}