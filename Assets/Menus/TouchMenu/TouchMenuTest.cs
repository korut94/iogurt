using DigitalRubyShared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMenuTest : MonoBehaviour {
    PanGestureRecognizer    m_panGesture;
    TapGestureRecognizer    m_tapGesture;

    void CreateTapGesture()
    {
        m_tapGesture = new TapGestureRecognizer();
        m_tapGesture.StateUpdated += TapGestureCallback;

        FingersScript.Instance.AddGesture(m_tapGesture);
    }

    void CreatePanGesture()
    {
        m_panGesture = new PanGestureRecognizer();
        m_panGesture.StateUpdated += PanGestureCallback;

        FingersScript.Instance.AddGesture(m_panGesture);
    }

    void PanGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {
            Debug.Log(string.Format("Panned, Location: {0}, {1}, Delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY));
        }
    }

    void TapGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Debug.Log(string.Format("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY));
        }
    }

    void Start()
    {
        CreatePanGesture();
        CreateTapGesture();
    }
}
