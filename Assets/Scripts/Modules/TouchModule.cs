using DigitalRubyShared;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Iogurt.Input.Touch
{
    sealed public class TouchModule : MonoBehaviour {
        Dictionary<Type, HashSet<MonoBehaviour>> m_client = new Dictionary<Type, HashSet<MonoBehaviour>>();

        TapGestureRecognizer        m_tapGesture = new TapGestureRecognizer();
        // TapGestureRecognizer        m_doubleTapGesture = new TapGestureRecognizer();
        // TapGestureRecognizer        m_tripleTapGesture = new TapGestureRecognizer();
        SwipeGestureRecognizer      m_swipeGesture = new SwipeGestureRecognizer();
        PanGestureRecognizer        m_panGesture = new PanGestureRecognizer();
        // ScaleGestureRecognizer      m_scaleGesture = new ScaleGestureRecognizer();
        // RotateGestureRecognizer     m_rotateGesture = new RotateGestureRecognizer();
        LongPressGestureRecognizer  m_longPressGesture = new LongPressGestureRecognizer();

        string[] m_clients = { "TouchArea", "VerticalViewlTouch", "HorizontalViewTouch" };

        Vector2 m_averagePanSpeed = Vector2.zero;
        int     m_countSpeedSample = 0;

        void LongPressGestureCallback(GestureRecognizer gesture)
        {
            foreach (var client in m_client[typeof(IUsesLongPressGesture)])
            {
                client.GetComponent<IUsesLongPressGesture>().LongPressGesture(gesture as LongPressGestureRecognizer);
            }
        }

        void PanGestureCallback(GestureRecognizer gesture)
        {
            if (!SwipeRecognitionOverPan(gesture))
            {
                foreach (var client in m_client[typeof(IUsesPanGesture)])
                {
                    client.GetComponent<IUsesPanGesture>().PanGesture(gesture as PanGestureRecognizer);
                }
            }
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

                    foreach (var client in m_client[typeof(IUsesSwipeGesture)])
                    {
                        client.GetComponent<IUsesSwipeGesture>().Swipe(direction, speed);
                    }
                }

                m_averagePanSpeed = Vector2.zero;
                m_countSpeedSample = 0;
            }

            return swipe;
        }

        void TapGestureCallback(GestureRecognizer gesture)
        {
            foreach (var client in m_client[typeof(IUsesTapGesture)])
            {
                client.GetComponent<IUsesTapGesture>().TapGesture(gesture as TapGestureRecognizer);
            }
        }

        void CreateLongPressGesture()
        {
            m_client.Add(typeof(IUsesLongPressGesture), new HashSet<MonoBehaviour>());
            FindClient<IUsesLongPressGesture>();

            m_longPressGesture.StateUpdated += LongPressGestureCallback;

            FingersScript.Instance.AddGesture(m_longPressGesture);
        }

        void CreatePanGesture()
        {
            m_client.Add(typeof(IUsesPanGesture), new HashSet<MonoBehaviour>());
            m_client.Add(typeof(IUsesSwipeGesture), new HashSet<MonoBehaviour>());
            FindClient<IUsesPanGesture>();
            FindClient<IUsesSwipeGesture>();

            m_panGesture.StateUpdated += PanGestureCallback;

            FingersScript.Instance.AddGesture(m_panGesture);
        }

        void CreateTapGesture()
        {
            m_client.Add(typeof(IUsesTapGesture), new HashSet<MonoBehaviour>());
            FindClient<IUsesTapGesture>();

            m_tapGesture.StateUpdated += TapGestureCallback;

            FingersScript.Instance.AddGesture(m_tapGesture);
        }

        void FindClient<T>()
        {
            // Just to test
            foreach (var name in m_clients)
            {
                var go = GameObject.Find(name);
                var component = go.GetComponent<T>() as MonoBehaviour;

                if (component != null) {
                    m_client[typeof(T)].Add(component);
                }
            }
        }

        bool? CaptureGestureHandler(GameObject obj)
        {
            if (m_clients.Contains(obj.name))
                return true;

            return false;
        }

        void Start() 
        {
            CreateLongPressGesture();
            CreatePanGesture();
            CreateTapGesture();
            FingersScript.Instance.CaptureGestureHandler = CaptureGestureHandler;
        }
    }
}

