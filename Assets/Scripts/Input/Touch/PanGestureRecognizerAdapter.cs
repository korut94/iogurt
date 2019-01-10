using DigitalRubyShared;
using UnityEngine;

namespace Iogurt.Input.Touch
{
    public sealed class PanGestureRecognizerAdapter : PanGestureRecognizer
    {
        Vector2 m_delta = Vector2.zero;
        Vector2 m_focus = Vector2.zero;
        GestureRecognizerState m_state;

        new public float DeltaX { get { return m_delta.x; } }
        new public float DeltaY { get { return m_delta.y; } }
        new public float FocusX { get { return m_focus.x; } }
        new public float FocusY { get { return m_focus.y; } }
        new public GestureRecognizerState State { get { return m_state; } }

        public void SetDelta(Vector2 delta) { m_delta = delta; }
        public void SetFocus(Vector2 focus) { m_focus = focus; }
        new public void SetState(GestureRecognizerState state) { m_state = state; }
    }
}

