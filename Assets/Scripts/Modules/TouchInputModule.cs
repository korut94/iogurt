using DigitalRubyShared;
using Iogurt.Input.Touch;
using Iogurt.Modules.Injection;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR;
using UnityEngine;
using UnityEngine.InputNew;

namespace Iogurt.Modules
{
    [RequireComponent(typeof(FingersScript))]
    sealed public class TouchInputModule : MonoBehaviour, IInterfaceProvider
    {
        /*
        class VRTrackpadAdapter : IVirtualTouch
        {
            AxisInputControl m_inputX;
            AxisInputControl m_inputY;
            Node m_node;

            public void ProcessAsTouchEvent(ProcessTouchDelegate processTouch, Dictionary<int, Vector2> prevTouchPosition)
            {
                if (m_inputX == null || m_inputY == null)
                    return;

                float x = m_inputX.rawValue;
                float y = m_inputY.rawValue;

                Vector2 prev;
                if (!prevTouchPosition.TryGetValue((int)m_node, out prev))
                {
                    prev.x = x;
                    prev.y = y;
                }

                // The trackpad is not pressed
                if (x == 0f && y == 0f && prev.x == 0f && prev.y == 0f)
                    return;

                DigitalRubyShared.TouchPhase phase;

                if (prev == Vector2.zero)
                {
                    phase = DigitalRubyShared.TouchPhase.Began;
                }
                else if (x == 0f && y == 0f)
                {
                    phase = DigitalRubyShared.TouchPhase.Ended;
                }
                else
                {
                    phase = DigitalRubyShared.TouchPhase.Moved;
                }

                prevTouchPosition[(int)m_node] = new Vector2(x, y);

                x = Mathf.Floor((x * 0.5f + 0.5f) * 2048f);
                y = Mathf.Floor((y * 0.5f + 0.5f) * 2048f);
                prev.x = Mathf.Floor((prev.x * 0.5f + 0.5f) * 2048f);
                prev.y = Mathf.Floor((prev.y * 0.5f + 0.5f) * 2048f);

                GestureTouch g = new GestureTouch((int)m_node, x, y, prev.x, prev.y, 0.0f, x, y, m_inputX.index + m_inputY.index, phase);
                processTouch(ref g);
            }

            public void UpdateAxes(Node node, AxisInputControl x, AxisInputControl y)
            {
                m_inputX = x;
                m_inputY = y;
                m_node = node;
            }
        }

        TapGestureRecognizer        m_tapGesture = new TapGestureRecognizer();
        // TapGestureRecognizer        m_doubleTapGesture = new TapGestureRecognizer();
        // TapGestureRecognizer        m_tripleTapGesture = new TapGestureRecognizer();
        // SwipeGestureRecognizer      m_swipeGesture = new SwipeGestureRecognizer();
        PanGestureRecognizer        m_panGesture = new PanGestureRecognizer();
        // ScaleGestureRecognizer      m_scaleGesture = new ScaleGestureRecognizer();
        // RotateGestureRecognizer     m_rotateGesture = new RotateGestureRecognizer();
        LongPressGestureRecognizer  m_longPressGesture = new LongPressGestureRecognizer();
        VRTrackpadAdapter           m_vrTrackpadAdapter = new VRTrackpadAdapter();

        Action<LongPressGestureRecognizer>              m_listenersForLongPress;
        Action<PanGestureRecognizer>                    m_listenersForPan;
        Action<TapGestureRecognizer>                    m_listenersForTap;
        Action<SwipeGestureRecognizerDirection, float>  m_listenersForSwipe;

        Vector2 m_averagePanSpeed = Vector2.zero;
        int     m_countSpeedSample = 0;

        void LongPressGestureCallback(GestureRecognizer gesture)
        {
            Debug.Log("Long Press");

            if (m_listenersForLongPress != null)
                m_listenersForLongPress(gesture as LongPressGestureRecognizer);
        }

        void PanGestureCallback(GestureRecognizer gesture)
        {
            Debug.Log("Pan: " + new Vector2(gesture.FocusX, gesture.FocusY));

            if (!SwipeRecognitionOverPan(gesture) && m_listenersForPan != null)
                m_listenersForPan(gesture as PanGestureRecognizer);
        }

        bool SwipeRecognitionOverPan(GestureRecognizer gesture)
        {
            var swipe = false;

            if (gesture.State == GestureRecognizerState.Executing)
            {
                m_averagePanSpeed = new Vector2(gesture.VelocityX, gesture.VelocityY);
                m_countSpeedSample++;
            }
            else if (gesture.State == GestureRecognizerState.Ended)
            {
                m_averagePanSpeed /= m_countSpeedSample;

                var direction = SwipeGestureRecognizerDirection.Any;
                var speed = 0f;
                var absXDiff = Mathf.Abs(m_averagePanSpeed.x);
                var absYDiff = Mathf.Abs(m_averagePanSpeed.y);
                var ratioX = (absYDiff == 0f) ? absXDiff : absXDiff / absYDiff;
                var ratioY = (absXDiff == 0f) ? absYDiff : absYDiff / absXDiff;
                
                if (absXDiff > absYDiff && ratioX > 1.5f)
                {
                    direction = (m_averagePanSpeed.x > 0) ? SwipeGestureRecognizerDirection.Right : SwipeGestureRecognizerDirection.Left;
                    speed = m_averagePanSpeed.x;
                }
                else if (absYDiff > absXDiff && ratioY > 1.5f)
                {
                    direction = (m_averagePanSpeed.y > 0) ? SwipeGestureRecognizerDirection.Up : SwipeGestureRecognizerDirection.Down;
                    speed = m_averagePanSpeed.y;
                }

                if (direction != SwipeGestureRecognizerDirection.Any)
                {
                    swipe = true;

                    if (m_listenersForSwipe != null)
                        m_listenersForSwipe(direction, speed);
                }

                m_averagePanSpeed = Vector2.zero;
                m_countSpeedSample = 0;
            }

            return swipe;
        }

        void TapGestureCallback(GestureRecognizer gesture)
        {
            Debug.Log("Tap: " + new Vector2(gesture.FocusX, gesture.FocusY));

            if (m_listenersForTap != null)
                m_listenersForTap(gesture as TapGestureRecognizer);
        }

        void CreateLongPressGesture()
        {
            // m_longPressGesture.MinimumDurationSeconds = 0.4f;
            // m_longPressGesture.MaximumNumberOfTouchesToTrack = 1;

            m_longPressGesture.StateUpdated += LongPressGestureCallback;
            FingersScript.Instance.AddGesture(m_longPressGesture);
        }

        void CreatePanGesture()
        {
            // m_panGesture.MinimumNumberOfTouchesToTrack = 1;
            // m_panGesture.ThresholdUnits = 0.0015f;
            m_panGesture.RequireGestureRecognizerToFail = m_tapGesture;

            m_panGesture.StateUpdated += PanGestureCallback;
            FingersScript.Instance.AddGesture(m_panGesture);
        }

        void CreateTapGesture()
        {
            // m_tapGesture.ThresholdUnits = 0.01f;

            m_tapGesture.StateUpdated += TapGestureCallback;
            FingersScript.Instance.AddGesture(m_tapGesture);
        }

        void Start()
        {
            CreateLongPressGesture();
            CreatePanGesture();
            CreateTapGesture();
            FingersScript.Instance.AddVirtualTouch(m_vrTrackpadAdapter);
        }
        */
        ///*
        float TimeTapInSeconds = 0.2f;

        PanGestureRecognizerAdapter m_panGesture = new PanGestureRecognizerAdapter();

        Action<PanGestureRecognizer> m_listenersForPan;
        Action<TapGestureRecognizer> m_listenersForTap;

        List<Vector2> m_inputHistory = new List<Vector2>();
        float m_timeStartingInputSequence = 0f;

        public void ConnectInterface(object target, object userData = null)
        {
            /*
            var longPress = target as ILongPressGesture;
            if (longPress != null)
            {
                m_listenersForLongPress += longPress.OnLongPressGesture;
            }
            */
            var panGesture = target as IPanGesture;
            if (panGesture != null)
            {
                m_listenersForPan += panGesture.OnPanGesture;
            }

            var tapGesture = target as ITapGesture;
            if (tapGesture != null)
            {
                m_listenersForTap += tapGesture.OnTapGesture;
            }
            /*
            var swipeGesture = target as ISwipeGesture;
            if (swipeGesture != null)
            {
                m_listenersForSwipe += swipeGesture.OnSwipeGesture;
            }*/
        }

        public void DisconnectInterface(object target, object userData = null)
        {/*
            var longPress = target as ILongPressGesture;
            if (longPress != null)
            {
                m_listenersForLongPress -= longPress.OnLongPressGesture;
            }
            */
            var panGesture = target as IPanGesture;
            if (panGesture != null)
            {
                m_listenersForPan -= panGesture.OnPanGesture;
            }

            var tapGesture = target as ITapGesture;
            if (tapGesture != null)
            {
                m_listenersForTap -= tapGesture.OnTapGesture;
            }
            /*
            var swipeGesture = target as ISwipeGesture;
            if (swipeGesture != null)
            {
                m_listenersForSwipe -= swipeGesture.OnSwipeGesture;
            }*/
        }
        
        public void UpdateAxes(Node node, AxisInputControl x, AxisInputControl y)
        {
            // m_vrTrackpadAdapter.UpdateAxes(node, x, y);

            Vector2 input = new Vector2(x.rawValue, y.rawValue);
            // There is at least a sequence in progress or it's starting now
            if (!(m_inputHistory.Count == 0 && input == Vector2.zero))
            {
                // History empty involve that it's the first input of the series
                if (m_inputHistory.Count == 0)
                {
                    m_timeStartingInputSequence = Time.time;
                }

                m_inputHistory.Add(input);

                float elapsedTime = Time.time - m_timeStartingInputSequence;
                bool isEndInput = m_inputHistory.Count > 0 && input == Vector2.zero;

                // Tap detected
                if (isEndInput && elapsedTime <= TimeTapInSeconds)
                {
                    m_listenersForTap(new TapGestureRecognizerAdapter());
                }
                // Pan decected, the user has hold down the finger more than the time expected for a tap
                else if (elapsedTime > TimeTapInSeconds)
                {
                    var delta = input - m_inputHistory[0];
                    var state = (isEndInput) ? GestureRecognizerState.Ended : GestureRecognizerState.Executing;

                    m_panGesture.SetDelta(delta);
                    m_panGesture.SetFocus(input);
                    m_panGesture.SetState(state);
                    m_listenersForPan(m_panGesture);
                }

                if (isEndInput)
                {
                    // Throw away the history since it's useless for the tap
                    m_inputHistory.Clear();
                    // Reset the timer
                    m_timeStartingInputSequence = 0f;
                }
            }
        }
    }
}
