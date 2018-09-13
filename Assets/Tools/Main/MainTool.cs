#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using DigitalRubyShared;
using UnityEditor.Experimental.EditorVR;
using UnityEngine;
using UnityEngine.InputNew;

namespace Iogurt.Tools
{
    [MainMenuItem("Iogurt", "Extensions", "Starting Iogurt Editor Extension")]
    sealed class MainTool : MonoBehaviour, ITool, IConnectInterfaces, IInstantiateMenuUI,
        IUsesRayOrigin, ICustomActionMap, IUsesNode
    {
        private class VRTrackpadAdapter : IVirtualTouch
        {
            private AxisInputControl    m_inputX;
            private AxisInputControl    m_inputY;
            private Node                m_node;

            public void ProcessAsTouchEvent(ProcessTouchDelegate processTouch, Dictionary<int, Vector2> prevTouchPosition)
            {
                if (m_inputX == null || m_inputY == null)
                    return;

                Vector2 prev;
                if (!prevTouchPosition.TryGetValue((int)m_node, out prev))
                {
                    prev = Vector2.zero;
                }

                float x = m_inputX.rawValue;
                float y = m_inputY.rawValue;

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

        [SerializeField]
        ActionMap m_actionMap;   
        [SerializeField]
        MainMenu m_menuPrefab;

        public ActionMap    actionMap { get { return m_actionMap; } }
        public bool         ignoreLocking { get { return false; } }
        public Transform    rayOrigin { get; set; }
        public Node         node { get; set; }

        GameObject                  m_toolMenu;
        LongPressGestureRecognizer  m_longPressGesture = new LongPressGestureRecognizer();
        PanGestureRecognizer        m_panGesture = new PanGestureRecognizer();
        SwipeGestureRecognizer      m_swipeGesture = new SwipeGestureRecognizer();
        TapGestureRecognizer        m_tapGesture = new TapGestureRecognizer();
        VRTrackpadAdapter           m_trackpadAdapter = new VRTrackpadAdapter();

        public void ProcessInput(ActionMapInput input, ConsumeControlDelegate consumeControl)
        {
            var mainMenuInput = (MainToolInput) input;

            m_trackpadAdapter.UpdateAxes(node, mainMenuInput.touchXAxis, mainMenuInput.touchYAxis);
            consumeControl(mainMenuInput.touchXAxis);
            consumeControl(mainMenuInput.touchYAxis);
        }

        void Start() {
            // Clear selection so we can't manipulate things
            UnityEditor.Selection.activeGameObject = null;

            m_toolMenu = this.InstantiateMenuUI(rayOrigin, m_menuPrefab);
            var mainMenu = m_toolMenu.GetComponent<MainMenu>();

            this.ConnectInterfaces(mainMenu, rayOrigin);

            CreateLongPressGesture();
            CreatePanGesture();
            // CreateSwipeGesture();
            CreateTapGesture();

            // m_swipeGesture.AllowSimultaneousExecution(m_panGesture);

            FingersScript.Instance.AddVirtualTouch(m_trackpadAdapter);
        }

        void CreateTapGesture()
        {
            m_tapGesture.ThresholdUnits = 0.01f;
            m_tapGesture.StateUpdated += gesture =>
            {
                if (gesture.State == GestureRecognizerState.Ended)
                {
                    Debug.Log("Tap");
                }
            };

            FingersScript.Instance.AddGesture(m_tapGesture);
        }

        void CreateSwipeGesture()
        {
            m_swipeGesture.MinimumDistanceUnits = 0.0001f;
            m_swipeGesture.MinimumSpeedUnits = 0.02f;
            m_swipeGesture.StateUpdated += gesture =>
            {
                if (gesture.State == GestureRecognizerState.Ended)
                {
                    Dictionary<SwipeGestureRecognizerDirection, string> direction = new Dictionary<SwipeGestureRecognizerDirection, string>()
                    {
                        { SwipeGestureRecognizerDirection.Up, "Up" },
                        { SwipeGestureRecognizerDirection.Down, "Down" },
                        { SwipeGestureRecognizerDirection.Left, "Left" },
                        { SwipeGestureRecognizerDirection.Right, "Rigth" },
                        { SwipeGestureRecognizerDirection.Any, "Unknow" }
                    };

                    Debug.Log("Swiped on direction "+ direction[m_swipeGesture.EndDirection]);
                }
            };

            FingersScript.Instance.AddGesture(m_swipeGesture);
        }

        void CreatePanGesture()
        {
            m_panGesture.MinimumNumberOfTouchesToTrack = 1;
            m_panGesture.RequireGestureRecognizerToFail = m_swipeGesture;
            m_panGesture.ThresholdUnits = 0.0015f;
            m_panGesture.StateUpdated += gesture =>
            {
                if (gesture.State == GestureRecognizerState.Executing)
                {
                    Debug.Log("Pan gesture");
                }
            };

            FingersScript.Instance.AddGesture(m_panGesture);
        }

        void CreateLongPressGesture()
        {
            m_longPressGesture.MinimumDurationSeconds = 0.4f;
            m_longPressGesture.MaximumNumberOfTouchesToTrack = 1;
            m_longPressGesture.StateUpdated += gesture =>
            {
                if (gesture.State == GestureRecognizerState.Began)
                {
                    Debug.Log(string.Format("Long press began: {0}, {1}", gesture.FocusX, gesture.FocusY));
                }
                else if (gesture.State == GestureRecognizerState.Executing)
                {
                    Debug.Log(string.Format("Long press moved: {0}, {1}", gesture.FocusX, gesture.FocusY));
                }
                else if (gesture.State == GestureRecognizerState.Ended)
                {
                    Debug.Log(string.Format("Long press end: {0}, {1}, delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY));
                }
            };

            FingersScript.Instance.AddGesture(m_longPressGesture);
        }
    }
}
#endif
