using DigitalRubyShared;
using UnityEngine;

namespace Iogurt.Input.Touch
{
    public sealed class PanGestureRecognizerAdapter : PanGestureRecognizer
    {
        Vector2 m_delta = Vector2.zero;
        GestureRecognizerState m_state;

        new public float DeltaX { get { return m_delta.x; } }
        new public float DeltaY { get { return m_delta.y; } }
        new public GestureRecognizerState State { get { return m_state; } }

        public PanGestureRecognizerAdapter(Vector2 delta, GestureRecognizerState state)
        {
            m_delta = delta;
            m_state = state;
        }
    }
}

